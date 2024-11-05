using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Hubs;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class DataFetchingBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataFetchingBackgroundService> _logger;
    private readonly IHubContext<UpdateReceiveTLIPHub> _hubContext;
    private List<AsnInformation> previousData = new List<AsnInformation>();
    private DateTime _lastRunDate = DateTime.MinValue;

    public DataFetchingBackgroundService(IServiceProvider serviceProvider, ILogger<DataFetchingBackgroundService> logger, IHubContext<UpdateReceiveTLIPHub> hubContext)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<ISchedulereceivedTLIPService>();

                    _logger.LogInformation("ExecuteAsync called at {Time}", DateTime.Now.ToString("T"));
                    _logger.LogInformation("_lastRunDate is {_lastRunDate.Date}", _lastRunDate.Date);

                    if (DateTime.Now.Date > _lastRunDate.Date)
                    {
                        await service.AddAllPlanDetailsToHistoryAsync();
                        _lastRunDate = DateTime.Now;
                    }
                    // Gọi hàm FetchData
                    await FetchData(service);

                    // Gọi hàm FetchDataDetail
                    await FetchDataDetail(service);

                    var incompleteActualReceived = await GetIncompleteActualReceived(service);

                    if (incompleteActualReceived != null)
                    {
                        foreach (var actualReceivedDTO in incompleteActualReceived)
                        {
                            if (!actualReceivedDTO.IsCompleted && actualReceivedDTO.CompletionPercentage < 100)
                            {
                                var formattedEnd = DateTime.Now;
                                actualReceivedDTO.ActualDeliveryTime = formattedEnd;

                                var actualReceived = MapToEntity(actualReceivedDTO);
                                await UpdateActualLeadTime(actualReceived, service);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching data.");
            }

            // Đợi 5 giây trước khi gọi lại
            await Task.Delay(5000, stoppingToken);
        }
    }



    private Actualreceivedtlip MapToEntity(ActualReceivedTLIPDTO dto)
    {
        return new Actualreceivedtlip
        {
            ActualReceivedId = dto.ActualReceivedId,
            ActualDeliveryTime = dto.ActualDeliveryTime,
            ActualLeadTime = dto.ActualLeadTime,
            SupplierCode = dto.SupplierCode,
            AsnNumber = dto.AsnNumber,
            DoNumber = dto.DoNumber,
            Invoice = dto.Invoice,
            IsCompleted = dto.IsCompleted,
            Actualdetailtlips = dto.ActualDetails.Select(d => new Actualdetailtlip
            {
                ActualDetailId = d.ActualDetailId,
                ActualReceivedId = d.ActualReceivedId
            }).ToList()
        };
    }


    private ActualReceivedTLIPDTO MapToDTO(Actualreceivedtlip actualReceived)
    {
        return new ActualReceivedTLIPDTO
        {
            ActualReceivedId = actualReceived.ActualReceivedId,
            ActualDeliveryTime = actualReceived.ActualDeliveryTime,
            ActualLeadTime = actualReceived.ActualLeadTime,
            SupplierCode = actualReceived.SupplierCode,
            AsnNumber = actualReceived.AsnNumber,
            DoNumber = actualReceived.DoNumber,
            Invoice = actualReceived.Invoice,
            SupplierName = actualReceived.SupplierCodeNavigation.SupplierName,
            CompletionPercentage = CalculateCompletionPercentage(actualReceived),
            IsCompleted = actualReceived.IsCompleted,
            ActualDetails = actualReceived.Actualdetailtlips.Select(detail => new ActualDetailTLIPDTO
            {
                ActualDetailId = detail.ActualDetailId,
                ActualReceivedId = detail.ActualReceivedId,
                PartNo = detail.PartNo,
                Quantity = detail.Quantity ?? 0,
                QuantityRemain = detail.QuantityRemain ?? 0
            }).ToList()
        };
    }

    private async Task FetchData(ISchedulereceivedTLIPService service)
    {
        _logger.LogInformation("FetchData called at {Time}", DateTime.Now.ToString("T"));
        //Lấy dữ liệu từ API

        var now = DateTime.Now;
        var nextData = await service.GetAsnInformationAsync(now);

        //var nextData = await ParseAsnInformationFromFileAsync();


        //_logger.LogInformation($"Next Data: {JsonSerializer.Serialize(nextData)}");

        foreach (var nextItem in nextData)
        {
            //Gán dữ liệu cũ cho biến previousItem
            var previousItem = previousData.FirstOrDefault(item =>
                (item.AsnNumber != null && item.AsnNumber == nextItem.AsnNumber) ||
                (item.AsnNumber == null && item.DoNumber != null && item.DoNumber == nextItem.DoNumber) ||
                (item.AsnNumber == null && item.DoNumber == null && item.Invoice != null && item.Invoice == nextItem.Invoice)
            );
            //Kiểm tra xem dữ liệu đã tồn tại trong database chưa
            var exists = await service.GetActualReceivedByDetailsAsync(new ActualReceivedTLIPDTO
            {
                SupplierCode = nextItem.SupplierCode,
                AsnNumber = nextItem.AsnNumber,
                DoNumber = nextItem.DoNumber,
                Invoice = nextItem.Invoice
            });

            if (exists == null)
            {
                //Nếu dữ liệu chưa tồn tại thì thêm vào database
                if (previousItem != null && !previousItem.ReceiveStatus && nextItem.ReceiveStatus)
                {
                    Console.WriteLine("Change status first!");

                    var actualReceived = new Actualreceivedtlip
                    {
                        ActualDeliveryTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                        SupplierCode = nextItem.SupplierCode,
                        AsnNumber = nextItem.AsnNumber,
                        DoNumber = nextItem.DoNumber,
                        Invoice = nextItem.Invoice,
                        IsCompleted = nextItem.IsCompleted
                    };

                    await service.AddActualReceivedAsync(actualReceived);
                    //Console.WriteLine("Add actual successfully in BackgroundService!");
                    //_logger.LogInformation($"SupplierCode: {actualReceived.SupplierCode}, ActualDeliveryTime: {actualReceived.ActualDeliveryTime}, AsnNumber: {actualReceived.AsnNumber}, DoNumber: {actualReceived.DoNumber}, Invoice: {actualReceived.Invoice}");

                    //Phải parse vì ActualDeliveryTime đang ở dạng MM/dd/yyyy HH:mm:ss
                    var formattedDateTime = actualReceived.ActualDeliveryTime.ToString("yyyy-MM-dd HH:mm:ss");
                    //_logger.LogInformation($"Formatted ActualDeliveryTime: {formattedDateTime}");

                    var actualReceivedEntry = await service.GetActualReceivedEntryAsync(
                                                            actualReceived.SupplierCode,
                                                             formattedDateTime,
                                                            actualReceived.AsnNumber,
                                                            actualReceived.DoNumber,
                                                            actualReceived.Invoice);
                    var actualReceivedDTO = MapToDTO(actualReceivedEntry);
                    //_logger.LogInformation($"ActualReceivedEntry: {JsonSerializer.Serialize(actualReceivedDTO)}");

                    if (actualReceivedEntry != null)
                    {
                        //Lấy asndetail
                        //var asnDetails = await ParseAsnDetailFromFile(
                        var asnDetails = await service.GetAsnDetailAsync(
                            actualReceivedEntry.AsnNumber,
                            actualReceivedEntry.DoNumber,
                            actualReceivedEntry.Invoice
                        );

                        if (asnDetails != null && asnDetails.Any())
                        {
                            foreach (var asnDetail in asnDetails)
                            {
                                var actualDetail = new Actualdetailtlip
                                {
                                    ActualReceivedId = actualReceivedEntry.ActualReceivedId,
                                    PartNo = asnDetail.PartNo,
                                    Quantity = asnDetail.Quantity,
                                    QuantityRemain = asnDetail.QuantityRemain
                                };

                                await service.AddActualDetailAsync(actualDetail);
                            }
                            //Lưu lịch sử cho ActualReceived
                            await service.AddAllActualToHistoryAsync(actualReceivedEntry.ActualReceivedId);
                            //Đảm bảo dữ liệu đã được add vào DB và gửi tín hiệu SignalR
                            await GetActualReceivedById(actualReceivedEntry.ActualReceivedId, service);
                        }
                        else
                        {
                            _logger.LogWarning("No ASN details found for AsnNumber: {0}, DoNumber: {1}, Invoice: {2}",
                                               actualReceivedEntry.AsnNumber, actualReceivedEntry.DoNumber, actualReceivedEntry.Invoice);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("actualReceivedEntry is null, unable to retrieve ASN details.");
                    }
                }

                //Kiểm tra xem dữ liệu trước đó đã hoàn thành chưa (đối với các trường hợp lo in được pallet mark)
                if (previousItem != null && !previousItem.IsCompleted && nextItem.IsCompleted)
                {
                    var actualReceived = await service.GetActualReceivedByDetailsAsync(new ActualReceivedTLIPDTO
                    {
                        SupplierCode = previousItem.SupplierCode,
                        AsnNumber = previousItem.AsnNumber,
                        DoNumber = previousItem.DoNumber,
                        Invoice = previousItem.Invoice
                    });

                    if (actualReceived != null)
                    {
                        await service.UpdateActualReceivedCompletionAsync(actualReceived.ActualReceivedId, true);
                        Console.WriteLine("Updated ActualReceived IsCompleted to true successfully.");
                    }
                }
            }

            //Nếu dữ liệu đã tồn tại trong DB thì kiểm tra xem dữ liệu mới có ReceiveStatus = true ko (đối với các trường hợp ko có DoNumber & Asn mà trùng Invoice)
            else if (exists != null && !nextItem.ReceiveStatus)
            {

                var duplicateData = (await service.GetActualReceivedAsyncByInfor(nextItem.AsnNumber, nextItem.DoNumber, nextItem.Invoice)).FirstOrDefault();
                if (duplicateData != null)
                {
                    if (!duplicateData.IsCompleted)
                    {
                        await service.UpdateActualReceivedCompletionAsync(duplicateData.ActualReceivedId, true);
                    }
                    await _hubContext.Clients.All.SendAsync("ErrorReceived", duplicateData.ActualReceivedId, duplicateData.SupplierCodeNavigation.SupplierName, duplicateData.IsCompleted);
                }
                Console.WriteLine("Duplicate data detected in API for ActualReceivedId: " + nextItem.AsnNumber + ", " + nextItem.DoNumber + ", " + nextItem.Invoice);
                Console.WriteLine("Updated ActualReceived IsCompleted to true successfully.");

            }

            //Nếu dữ liệu đã tồn tại thì kiểm tra xem dữ liệu mới nhất có IsCompleted = true ko (đối với các trường hợp ko đc quản lý = pallet mark)
            else if (exists != null && nextItem.IsCompleted)
            {
                var data = (await service.GetActualReceivedAsyncByInfor(nextItem.AsnNumber, nextItem.DoNumber, nextItem.Invoice)).FirstOrDefault();
                if (!data.IsCompleted)
                {
                    await service.UpdateActualReceivedCompletionAsync(exists.ActualReceivedId, true);
                }

            }
        }

        previousData = nextData.ToList();
    }


    private async Task<List<AsnInformation>> ParseAsnInformationFromFileAsync()
    {
        // string filePath = @"F:\FU\Semester_5\PRN212\Self_Study\DS_RIG\RIG\demoTLIP.txt";
        string filePath = @"D:\Project Stock Delivery\RIG\RIG\demoTLIP.txt";
        var asnInformationList = new List<AsnInformation>();

        using (var reader = new StreamReader(filePath))
        {
            var fileContent = await reader.ReadToEndAsync();
            var jsonDocument = JsonDocument.Parse(fileContent);

            foreach (var element in jsonDocument.RootElement.GetProperty("data").GetProperty("result").EnumerateArray())
            {
                var asnInformation = new AsnInformation
                {
                    AsnNumber = element.GetProperty("asnNumber").GetString(),
                    DoNumber = element.GetProperty("doNumber").GetString(),
                    Invoice = element.GetProperty("invoice").GetString(),
                    SupplierCode = element.GetProperty("supplierCode").GetString(),
                    SupplierName = element.GetProperty("supplierName").GetString(),
                    EtaDate = element.GetProperty("etaDate").GetDateTime(),
                    EtaDateString = element.GetProperty("etaDateString").GetString(),
                    ReceiveStatus = element.GetProperty("receiveStatus").GetBoolean(),
                    IsCompleted = element.GetProperty("isCompleted").GetBoolean()
                };

                asnInformationList.Add(asnInformation);
            }
        }

        return asnInformationList;
    }

    private async Task<List<AsnDetailData>> ParseAsnDetailFromFile(string asnNumber, string doNumber, string invoice)
    {
        string filePath = @"D:\Project Stock Delivery\RIG\RIG\demoDetailTLIP.txt";
        //string filePath = @"F:\FU\Semester_5\PRN212\Self_Study\DS_RIG\RIG\demoDetailTLIP.txt";

        using (var reader = new StreamReader(filePath))
        {
            var fileContent = await reader.ReadToEndAsync();
            var jsonDocument = JsonDocument.Parse(fileContent);
            var asnDetailList = new List<AsnDetailData>();

            foreach (var element in jsonDocument.RootElement.GetProperty("data").GetProperty("result").EnumerateArray())
            {
                // Check if the input parameters match the file data
                var isMatching = (!string.IsNullOrEmpty(asnNumber) && element.GetProperty("asnNumber").GetString() == asnNumber) ||
                                 (string.IsNullOrEmpty(asnNumber) && !string.IsNullOrEmpty(doNumber) && element.GetProperty("doNumber").GetString() == doNumber) ||
                                 (string.IsNullOrEmpty(asnNumber) && string.IsNullOrEmpty(doNumber) && !string.IsNullOrEmpty(invoice) && element.GetProperty("invoice").GetString() == invoice);


                if (isMatching)
                {
                    asnDetailList.Add(new AsnDetailData
                    {
                        PartNo = element.GetProperty("partNo").GetString(),
                        AsnNumber = element.GetProperty("asnNumber").GetString(),
                        DoNumber = element.GetProperty("doNumber").GetString(),
                        Invoice = element.GetProperty("invoice").GetString(),
                        Quantity = element.GetProperty("quantiy").GetInt32(),
                        QuantityRemain = element.GetProperty("quantityRemain").GetInt32()
                    });
                }
            }

            return asnDetailList;
        }
    }




    private async Task FetchDataDetail(ISchedulereceivedTLIPService service)
    {
        var actualReceivedList = await GetIncompleteActualReceived(service);
        foreach (var actualReceived in actualReceivedList)
        {
            var asnDetails = await service.GetAsnDetailAsync(
            //var asnDetails = await ParseAsnDetailFromFile(
            actualReceived.AsnNumber ?? string.Empty,
            actualReceived.DoNumber ?? string.Empty,
            actualReceived.Invoice ?? string.Empty
        );
            //_logger.LogInformation($"ASN Details: {JsonSerializer.Serialize(asnDetails)}");
            if (asnDetails != null && asnDetails.Any())
            {
                foreach (var asnDetail in asnDetails)
                {
                    if (asnDetail.QuantityRemain == 0)
                    {
                        await service.UpdateActualDetailTLIPAsync(asnDetail.PartNo, actualReceived.ActualReceivedId, 0);
                    }
                }
            }
        }
    }

    public async Task GetActualReceivedById(int actualReceivedId, ISchedulereceivedTLIPService service)
    {
        var actualReceivedList = await service.GetAllActualReceivedAsyncById(actualReceivedId);
        var actualReceivedDTO = actualReceivedList.Select(ar => new ActualReceivedTLIPDTO
        {
            ActualReceivedId = ar.ActualReceivedId,
            ActualDeliveryTime = ar.ActualDeliveryTime,
            ActualLeadTime = ar.ActualLeadTime,
            SupplierCode = ar.SupplierCode,
            SupplierName = ar.SupplierCodeNavigation?.SupplierName,
            AsnNumber = ar.AsnNumber,
            DoNumber = ar.DoNumber,
            Invoice = ar.Invoice,
            ActualDetails = ar.Actualdetailtlips.Select(detail => new ActualDetailTLIPDTO
            {
                ActualDetailId = detail.ActualDetailId,
                PartNo = detail.PartNo,
                Quantity = detail.Quantity ?? 0,
                QuantityRemain = detail.QuantityRemain ?? 0,
                ActualReceivedId = detail.ActualReceivedId
            }).ToList(),
            CompletionPercentage = CalculateCompletionPercentage(ar)
        }).FirstOrDefault();

        if (actualReceivedDTO == null)
        {
            _logger.LogInformation("ActualReceivedDTO not found for ID: {ActualReceivedId}", actualReceivedId);
            return;
        }

        //_logger.LogInformation("ActualReceivedDTO: {ActualReceivedDTO}", JsonSerializer.Serialize(actualReceivedDTO));

        await _hubContext.Clients.All.SendAsync("UpdateCalendar", actualReceivedDTO);
    }


    private async Task<List<ActualReceivedTLIPDTO>> GetIncompleteActualReceived(ISchedulereceivedTLIPService service)
    {
        var actualReceivedList = await service.GetAllActualReceivedAsync();
        var incompleteActualReceivedDTOList = actualReceivedList
            .Where(actualReceived => CalculateCompletionPercentage(actualReceived) < 100)
            .Select(ar => new ActualReceivedTLIPDTO
            {
                ActualReceivedId = ar.ActualReceivedId,
                ActualDeliveryTime = ar.ActualDeliveryTime,
                ActualLeadTime = ar.ActualLeadTime,
                SupplierCode = ar.SupplierCode,
                SupplierName = ar.SupplierCodeNavigation?.SupplierName,
                AsnNumber = ar.AsnNumber,
                DoNumber = ar.DoNumber,
                Invoice = ar.Invoice,
                IsCompleted = ar.IsCompleted,
                ActualDetails = ar.Actualdetailtlips.Select(detail => new ActualDetailTLIPDTO
                {
                    ActualDetailId = detail.ActualDetailId,
                    PartNo = detail.PartNo,
                    Quantity = detail.Quantity ?? 0,
                    QuantityRemain = detail.QuantityRemain ?? 0,
                    ActualReceivedId = detail.ActualReceivedId
                }).ToList(),
                CompletionPercentage = CalculateCompletionPercentage(ar)
            }).ToList();

        return incompleteActualReceivedDTOList;
    }

    private double CalculateCompletionPercentage(Actualreceivedtlip actualReceived)
    {
        var totalItems = actualReceived.Actualdetailtlips.Count;
        var completedItems = actualReceived.Actualdetailtlips.Count(detail => detail.QuantityRemain == 0);

        if (totalItems == 0) return 0;
        return (completedItems / (double)totalItems) * 100;
    }

    public async Task UpdateActualLeadTime(Actualreceivedtlip actualReceived, ISchedulereceivedTLIPService service)
    {
        if (actualReceived == null || actualReceived.ActualReceivedId <= 0)
        {
            _logger.LogError("Invalid data.");
            return;
        }

        try
        {
            // Tìm bản ghi actualReceived dựa trên actualReceivedId
            var existingActualReceived = await service.GetActualReceivedWithSupplierAsync(actualReceived.ActualReceivedId);

            if (existingActualReceived != null)
            {
                // Tính toán ActualLeadTime là chênh lệch giữa thời gian kết thúc và ActualDeliveryTime
                var endDateTime = actualReceived.ActualDeliveryTime;
                var leadTime = endDateTime - existingActualReceived.ActualDeliveryTime;
                existingActualReceived.ActualLeadTime = TimeOnly.FromTimeSpan(leadTime);

                // Lưu cập nhật
                await service.UpdateActualReceivedAsync(existingActualReceived);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ActualLeadTime.");
        }
    }
}
