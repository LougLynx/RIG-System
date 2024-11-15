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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class DataFetchingBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataFetchingBackgroundService> _logger;
    private readonly IHubContext<UpdateReceiveTLIPHub> _hubContext;
    private List<AsnInformation> previousData = new List<AsnInformation>();
    private DateOnly _lastRunDate = DateOnly.MinValue;

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
                    _logger.LogInformation("_lastRunDate is {_lastRunDate.Date}", _lastRunDate);

                    var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

                    if (_lastRunDate != currentDate)
                    {
                        _lastRunDate = currentDate;

                        await service.AddAllPlanDetailsToHistoryAsync();
                    }
                    // Gọi hàm FetchData
                    await FetchData(service);

                    // Gọi hàm FetchDataDetail
                    await FetchDataDetail(service);

                    var incompleteActualReceived = await service.GetIncompleteActualReceived();

                    if (incompleteActualReceived != null)
                    {
                        foreach (var actualReceived in incompleteActualReceived)
                        {
                            await service.UpdateActualLeadTime(actualReceived, DateTime.Now);
                            var actualReceivedDTO = service.MapToActualReceivedTLIPDTO(actualReceived);
                            await _hubContext.Clients.All.SendAsync("UpdateLeadtime", actualReceivedDTO);
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


    private async Task FetchData(ISchedulereceivedTLIPService service)
    {
        _logger.LogInformation("FetchData called at {Time}", DateTime.Now.ToString("T"));

        //Lấy dữ liệu từ API
        var now = DateTime.Now;
        var nextData = await service.GetAsnInformationAsync(now);

        /****
         ***
         **
         */
        //var nextData = await ParseAsnInformationFromFileAsync();

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
                    //Kiểm tra SupplierCode theo TagName VD: KCN, HCM,...
                    var tagNameRules = await service.GetAllTagNameRuleAsync();
                    //(Nếu không có TagName thì sẽ lấy chính SupplierCode)
                    string tagName = nextItem.SupplierCode;
                    foreach (var rule in tagNameRules)
                    {
                        if (rule.SupplierCode == nextItem.SupplierCode)
                        {
                            tagName = rule.TagName;
                            break;
                        }
                    }
                    var actualReceived = new Actualreceivedtlip
                    {
                        ActualDeliveryTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                        SupplierCode = nextItem.SupplierCode,
                        AsnNumber = nextItem.AsnNumber,
                        DoNumber = nextItem.DoNumber,
                        Invoice = nextItem.Invoice,
                        IsCompleted = nextItem.IsCompleted,
                        TagName = tagName
                    };

                    await service.AddActualReceivedAsync(actualReceived);

                    //Phải parse vì ActualDeliveryTime đang ở dạng MM/dd/yyyy HH:mm:ss
                    var formattedDateTime = actualReceived.ActualDeliveryTime.ToString("yyyy-MM-dd HH:mm:ss");

                    var actualReceivedEntry = await service.GetActualReceivedEntryAsync(
                                                            actualReceived.SupplierCode,
                                                             formattedDateTime,
                                                            actualReceived.AsnNumber,
                                                            actualReceived.DoNumber,
                                                            actualReceived.Invoice);
                    var actualReceivedDTO = service.MapToActualReceivedTLIPDTO(actualReceivedEntry);

                    if (actualReceivedEntry != null)
                    {
                        //Lấy asndetail
                        //var asnDetails = await ParseAsnDetailFromFile(
                        /****
                         ***
                         **
                         */
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
                                    QuantityRemain = asnDetail.QuantityRemain,
                                    QuantityScan = asnDetail.QuantityScan
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

            }

            //Nếu dữ liệu đã tồn tại thì kiểm tra xem dữ liệu mới nhất có IsCompleted = true ko (đối với các trường hợp ko đc quản lý = pallet mark)
            else if (exists != null && nextItem.IsCompleted)
            {
                var data = (await service.GetActualReceivedAsyncByInfor(nextItem.AsnNumber, nextItem.DoNumber, nextItem.Invoice)).FirstOrDefault();
                if (!data.IsCompleted)
                {
                    await service.UpdateActualReceivedCompletionAsync(exists.ActualReceivedId, true);
                    await _hubContext.Clients.All.SendAsync("UpdateColorDone", exists.ActualReceivedId);

                }

            }
        }

        previousData = nextData.ToList();
    }

    private async Task FetchDataDetail(ISchedulereceivedTLIPService service)
    {
        var actualReceivedList = await service.GetIncompleteActualReceived();
        foreach (var actualReceived in actualReceivedList)
        {
            var asnDetails = await service.GetAsnDetailAsync(
                actualReceived.AsnNumber ?? string.Empty,
                actualReceived.DoNumber ?? string.Empty,
                actualReceived.Invoice ?? string.Empty);

            if (asnDetails != null && asnDetails.Any())
            {
                // Kiểm tra xem tất cả các record trong asnDetails có QuantityRemain = 0 không (API)
                bool allAsnDetailsQuantityRemainZero = asnDetails.All(ad => ad.QuantityRemain == 0);

                if (allAsnDetailsQuantityRemainZero)
                {
                    var asnDetailsInDataBase = await service.GetAsnDetailInDataBaseAsync(
                        actualReceived.AsnNumber ?? string.Empty,
                        actualReceived.DoNumber ?? string.Empty,
                        actualReceived.Invoice ?? string.Empty);

                    //Kiểm tra xem tất cả các record trong DB có trong API có QuantityRemain = 0 không (DB)
                    bool allMatchingAsnDetailsInDataBaseQuantityRemainZero = asnDetailsInDataBase
                        .SelectMany(dbAd => dbAd.Actualdetailtlips)
                        .Where(dbDetail => asnDetails.Any(ad => ad.PartNo == dbDetail.PartNo))
                        .All(dbDetail => dbDetail.QuantityRemain == 0);

                    // Gửi tín hiệu SignalR nếu quantityRemain trong API = 0 và trong DB = 0
                    if (allMatchingAsnDetailsInDataBaseQuantityRemainZero)
                    {
                        await _hubContext.Clients.All.SendAsync("HandleReceived", actualReceived.ActualReceivedId);
                    }
                }
                else
                {
                    foreach (var asnDetail in asnDetails)
                    {
                        if (asnDetail.QuantityRemain == 0)
                        {
                            await service.UpdateActualDetailTLIPAsync(asnDetail.PartNo, actualReceived.ActualReceivedId, 0, null);
                        }

                        if (asnDetail.QuantityScan == asnDetail.Quantity || asnDetail.QuantityScan == asnDetail.Quantity - 1)
                        {
                            await service.UpdateActualDetailTLIPAsync(asnDetail.PartNo, actualReceived.ActualReceivedId, null, asnDetail.QuantityScan);
                        }
                    }
                }
            }
        }
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
                        QuantityRemain = element.GetProperty("quantityRemain").GetInt32(),
                        QuantityScan = element.GetProperty("quantityScan").GetInt32()
                    });
                }
            }

            return asnDetailList;
        }
    }

    public async Task GetActualReceivedById(int actualReceivedId, ISchedulereceivedTLIPService service)
    {
        var actualReceivedList = await service.GetAllActualReceivedAsyncById(actualReceivedId);
        var actualReceivedDTO = actualReceivedList.Select(ar =>
        {
            return service.MapToActualReceivedTLIPDTO(ar);
        }).FirstOrDefault();

        if (actualReceivedDTO == null)
        {
            _logger.LogInformation("ActualReceivedDTO not found for ID: {ActualReceivedId}", actualReceivedId);
            return;
        }

        await _hubContext.Clients.All.SendAsync("UpdateCalendar", actualReceivedDTO);
    }

}
