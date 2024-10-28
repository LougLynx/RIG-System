using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class DataFetchingBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataFetchingBackgroundService> _logger;
    private List<AsnInformation> previousData = new List<AsnInformation>();

    public DataFetchingBackgroundService(IServiceProvider serviceProvider, ILogger<DataFetchingBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
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

                    // Gọi hàm FetchData
                    await FetchData(service);

                    // Gọi hàm FetchDataDetail
                    await FetchDataDetail(service);
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



    /*  private async Task FetchData(ISchedulereceivedTLIPService service)
      {
          _logger.LogInformation("FetchData called at {Time}", DateTime.Now.ToString("T"));
          var now = DateTime.Now;
          var nextData = await service.GetAsnInformationAsync(now);

          foreach (var nextItem in nextData)
          {
              var previousItem = previousData.FirstOrDefault(item =>
                  (item.AsnNumber != null && item.AsnNumber == nextItem.AsnNumber) ||
                  (item.AsnNumber == null && item.DoNumber != null && item.DoNumber == nextItem.DoNumber) ||
                  (item.AsnNumber == null && item.DoNumber == null && item.Invoice != null && item.Invoice == nextItem.Invoice)
              );

              var exists = await service.GetActualReceivedByDetailsAsync(new ActualReceivedTLIPDTO
              {
                  SupplierCode = nextItem.SupplierCode,
                  AsnNumber = nextItem.AsnNumber,
                  DoNumber = nextItem.DoNumber,
                  Invoice = nextItem.Invoice
              });

              if (exists == null)
              {
                  if (previousItem != null && !previousItem.ReceiveStatus && nextItem.ReceiveStatus)
                  {
                      var actualReceived = new Actualreceivedtlip
                      {
                          ActualDeliveryTime = now,
                          SupplierCode = nextItem.SupplierCode,
                          AsnNumber = nextItem.AsnNumber,
                          DoNumber = nextItem.DoNumber,
                          Invoice = nextItem.Invoice,
                          IsCompleted = nextItem.IsCompleted
                      };

                      await service.AddActualReceivedAsync(actualReceived);

                      var actualReceivedEntry = await service.GetActualReceivedEntryAsync(actualReceived.SupplierCode, actualReceived.ActualDeliveryTime, actualReceived.AsnNumber);

                      var asnDetails = await service.GetAsnDetailAsync(actualReceivedEntry.AsnNumber, actualReceivedEntry.DoNumber, actualReceivedEntry.Invoice);

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
                  }

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
              else
              {
                  if (!nextItem.ReceiveStatus)
                  {
                      Console.WriteLine("Duplicate data detected in API for ActualReceivedId: " + nextItem.AsnNumber + ", " + nextItem.DoNumber + ", " + nextItem.Invoice + ", " + exists.ActualReceivedId);
                      await service.UpdateActualReceivedCompletionAsync(exists.ActualReceivedId, true);
                      Console.WriteLine("Updated ActualReceived IsCompleted to true successfully.");
                  }
              }
          }

          previousData = nextData.ToList();
      }
  */
    private async Task FetchData(ISchedulereceivedTLIPService service)
    {
        _logger.LogInformation("FetchData called at {Time}", DateTime.Now.ToString("T"));

        // Sử dụng ParseAsnInformationFromFileAsync thay vì GetAsnInformationAsync
        var nextData = await ParseAsnInformationFromFileAsync();

        foreach (var nextItem in nextData)
        {
            var previousItem = previousData.FirstOrDefault(item =>
                (item.AsnNumber != null && item.AsnNumber == nextItem.AsnNumber) ||
                (item.AsnNumber == null && item.DoNumber != null && item.DoNumber == nextItem.DoNumber) ||
                (item.AsnNumber == null && item.DoNumber == null && item.Invoice != null && item.Invoice == nextItem.Invoice)
            );

            var exists = await service.GetActualReceivedByDetailsAsync(new ActualReceivedTLIPDTO
            {
                SupplierCode = nextItem.SupplierCode,
                AsnNumber = nextItem.AsnNumber,
                DoNumber = nextItem.DoNumber,
                Invoice = nextItem.Invoice
            });

            if (exists == null)
            {
                if (previousItem != null && !previousItem.ReceiveStatus && nextItem.ReceiveStatus)
                {
                    Console.WriteLine("Change status first!");

                    var actualReceived = new Actualreceivedtlip
                    {
                        ActualDeliveryTime = DateTime.Now,
                        SupplierCode = nextItem.SupplierCode,
                        AsnNumber = nextItem.AsnNumber,
                        DoNumber = nextItem.DoNumber,
                        Invoice = nextItem.Invoice,
                        IsCompleted = nextItem.IsCompleted
                    };

                    await service.AddActualReceivedAsync(actualReceived);
                    Console.WriteLine("Add actual successfully!");

                    var actualReceivedEntry = await service.GetActualReceivedEntryAsync(actualReceived.SupplierCode, actualReceived.ActualDeliveryTime, actualReceived.AsnNumber, actualReceived.DoNumber, actualReceived.Invoice);

                    // Gọi ParseAsnDetailFromFile chấp nhận null cho các tham số
                    if (actualReceivedEntry != null)
                    {
                        var asnDetails = await ParseAsnDetailFromFile(
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
            else
            {
                if (!nextItem.ReceiveStatus)
                {
                    Console.WriteLine("Duplicate data detected in API for ActualReceivedId: " + nextItem.AsnNumber + ", " + nextItem.DoNumber + ", " + nextItem.Invoice + ", " + exists.ActualReceivedId);
                    await service.UpdateActualReceivedCompletionAsync(exists.ActualReceivedId, true);
                    Console.WriteLine("Updated ActualReceived IsCompleted to true successfully.");
                }
            }
        }

        previousData = nextData.ToList();
    }


    private async Task<List<AsnInformation>> ParseAsnInformationFromFileAsync()
    {
        string filePath = @"F:\FU\Semester_5\PRN212\Self_Study\DS_RIG\RIG\demoTLIP.txt";
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
        string filePath = @"F:\FU\Semester_5\PRN212\Self_Study\DS_RIG\RIG\demoDetailTLIP.txt";
        var asnDetailList = new List<AsnDetailData>();

        using (var reader = new StreamReader(filePath))
        {
            var fileContent = await reader.ReadToEndAsync();
            var jsonDocument = JsonDocument.Parse(fileContent);

            // Kiểm tra và lấy RootElement "data"
            if (!jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
            {
                _logger.LogError("Key 'data' not found in the JSON file.");
                return asnDetailList;
            }

            // Kiểm tra và lấy element "result"
            if (!dataElement.TryGetProperty("result", out var resultElement))
            {
                _logger.LogError("Key 'result' not found in the JSON file.");
                return asnDetailList;
            }

            // Lặp qua từng phần tử trong "result"
            foreach (var element in resultElement.EnumerateArray())
            {
                bool isMatching = false;

                // Kiểm tra điều kiện khớp theo các tham số có giá trị
                if (!string.IsNullOrEmpty(asnNumber) && element.TryGetProperty("asnNumber", out var asnNumberElement) && asnNumberElement.GetString() == asnNumber)
                {
                    isMatching = true;
                }
                else if (!string.IsNullOrEmpty(doNumber) && element.TryGetProperty("doNumber", out var doNumberElement) && doNumberElement.GetString() == doNumber)
                {
                    isMatching = true;
                }
                else if (!string.IsNullOrEmpty(invoice) && element.TryGetProperty("invoice", out var invoiceElement) && invoiceElement.GetString() == invoice)
                {
                    isMatching = true;
                }

                // Nếu có ít nhất một điều kiện khớp, thêm vào danh sách
                if (isMatching)
                {
                    if (element.TryGetProperty("partNo", out var partNo) &&
                        element.TryGetProperty("quantity", out var quantity) &&
                        element.TryGetProperty("quantityRemain", out var quantityRemain))
                    {
                        asnDetailList.Add(new AsnDetailData
                        {
                            PartNo = partNo.GetString(),
                            AsnNumber = asnNumber,
                            DoNumber = doNumber,
                            Invoice = invoice,
                            Quantity = quantity.GetInt32(),
                            QuantityRemain = quantityRemain.GetInt32()
                        });
                    }
                    else
                    {
                        _logger.LogWarning("One or more expected keys are missing for a detail item.");
                    }
                }
            }
        }

        return asnDetailList;
    }



    /* private async Task FetchDataDetail(ISchedulereceivedTLIPService service)
     {
         var actualReceivedList = await GetIncompleteActualReceived(service);
         foreach (var actualReceived in actualReceivedList)
         {
             var asnDetails = await service.GetAsnDetailAsync(
                 actualReceived.AsnNumber ?? string.Empty,
                 actualReceived.DoNumber ?? string.Empty,
                 actualReceived.Invoice ?? string.Empty
             );

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
     }*/

    private async Task FetchDataDetail(ISchedulereceivedTLIPService service)
    {
        _logger.LogInformation("FetchDataDetail called at {Time}", DateTime.Now.ToString("T"));

        var actualReceivedList = await GetIncompleteActualReceived(service);
        if (actualReceivedList != null && actualReceivedList.Any())
            foreach (var actualReceived in actualReceivedList)
            {
                {
                    // Thay thế GetAsnDetailAsync bằng ParseAsnDetailFromFile
                    var asnDetails = await ParseAsnDetailFromFile(
                        actualReceived.AsnNumber ?? string.Empty,
                        actualReceived.DoNumber ?? string.Empty,
                        actualReceived.Invoice ?? string.Empty
                    );

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
        else
        {
            _logger.LogError("Failed to fetch incomplete actual received data.");
        }
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
}
