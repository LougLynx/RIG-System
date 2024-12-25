using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Hubs;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Services;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using log4net;
namespace DataFetchingWorkerService
{
    public class DataTLIPWorker : BackgroundService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DataTLIPWorker));

        private readonly ILogger<DataTLIPWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private HubConnection _hubConnection;
        private List<AsnInformation> previousData = new List<AsnInformation>();
        private DateOnly _lastRunDate = DateOnly.MinValue;


        public DataTLIPWorker(IServiceProvider serviceProvider, ILogger<DataTLIPWorker> logger, IConfiguration configuration)

        {
            log.Info("Init DataTLIPWorker...");

            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
            var hubUrl = _configuration.GetSection("HubConnection:HubUrl").Value;
            _logger.LogInformation("URL to SignalR Hub is: {hubUrl}.", hubUrl);
            _hubConnection = new HubConnectionBuilder()
                                .WithUrl(hubUrl)
                                .WithAutomaticReconnect()
                                .Build();
           
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // K?t n?i t?i SignalR Hub
                await _hubConnection.StartAsync(stoppingToken);
                _logger.LogInformation("Connected to SignalR Hub.");
                log.Info("Connected to SignalR Hub.");


                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    _logger.LogInformation("SignalR Hub connection is ACTIVE. State: {State}. ", _hubConnection.State);
                }
                else
                {
                    _logger.LogWarning("SignalR Hub connection is NOT ACTIVE. State: {State}. ", _hubConnection.State);
                }

                _logger.LogInformation("Starting DataTLIPWorker...");
                log.Info("Starting DataTLIPWorker...");

                

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

                            // G?i c?c h?m x? l? logic
                            await FetchData(service);
                            await FetchDataDetail(service);
                            await FetchStorageDetail(service);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in ExecuteAsync loop.");
                        log.Error("Error in ExecuteAsync loop.", ex);
                    }

                    // Delay tr??c khi g?i l?i
                    await Task.Delay(5000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start or connect to SignalR Hub.");
                log.Error("Failed to start or connect to SignalR Hub.", ex);

            }
            finally
            {
                await _hubConnection.DisposeAsync();
            }
        }

        private async Task FetchData(ISchedulereceivedTLIPService service)
        {
            _logger.LogInformation("FetchData called at {Time}", DateTime.Now.ToString("T"));

            //L?y d? li?u t? API
            var now = DateTime.Now;
            var nextData = await service.GetAsnInformationAsync(now);

            /****
             ***
             **
             *?o?n n?y d?ng file txt test
             */
            //var nextData = await ParseAsnInformationFromFileAsync();

            foreach (var nextItem in nextData)
            {
                //G?n d? li?u cho bi?n previousItem
                var previousItem = previousData.FirstOrDefault(item =>
                    (item.AsnNumber != null && item.AsnNumber == nextItem.AsnNumber) ||
                    (item.AsnNumber == null && item.DoNumber != null && item.DoNumber == nextItem.DoNumber) ||
                    (item.AsnNumber == null && item.DoNumber == null && item.Invoice != null && item.Invoice == nextItem.Invoice)
                );
                //Ki?m tra xem d? li?u ?? c? trong database ch?a
                var exists = await service.GetActualReceivedByDetailsAsync(new ActualReceivedTLIPDTO
                {
                    SupplierCode = nextItem.SupplierCode,
                    AsnNumber = nextItem.AsnNumber,
                    DoNumber = nextItem.DoNumber,
                    Invoice = nextItem.Invoice
                });

                if (exists == null)
                {
                    //N?u d? li?u ch?a c? th? th?m v?o database
                    if (previousItem != null && !previousItem.ReceiveStatus && nextItem.ReceiveStatus)
                    {
                        var currentPlan = await service.GetCurrentPlanAsync();
                        //Ki?m tra SupplierCode theo TagName VD: KCN, HCM,...
                        var tagNameRules = await service.GetAllTagNameRuleAsync();
                        //(N?u kh?ng c? TagName th? s? l?y ch?nh SupplierCode)
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
                            TagName = tagName,
                            PlanId = currentPlan.PlanId
                        };

                        await service.AddActualReceivedAsync(actualReceived);

                        //Ph?i parse v? ActualDeliveryTime ?ang ? d?ng MM/dd/yyyy HH:mm:ss
                        var formattedDateTime = actualReceived.ActualDeliveryTime.ToString("yyyy-MM-dd HH:mm:ss");

                        var actualReceivedEntry = await service.GetActualReceivedEntryAsync(
                                                                actualReceived.SupplierCode,
                                                                 formattedDateTime,
                                                                actualReceived.AsnNumber,
                                                                actualReceived.DoNumber,
                                                                actualReceived.Invoice);


                        if (actualReceivedEntry != null)
                        {
                            //L?y asndetail
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
                                        QuantityScan = asnDetail.QuantityScan,
                                        StockInStatus = asnDetail.StockInStatus,
                                        StockInLocation = asnDetail.StockInLocation,
                                    };

                                    await service.AddActualDetailAsync(actualDetail);
                                }

                                //??m b?o d? li?u ?? ???c add v?o DB v? g?i t?n hi?u SignalR
                                await GetActualReceivedById(actualReceivedEntry.ActualReceivedId, service);
                            }
                            else
                            {
                                _logger.LogWarning("No ASN details found for AsnNumber: {0}, DoNumber: {1}, Invoice: {2}",
                                                   actualReceivedEntry.AsnNumber, actualReceivedEntry.DoNumber, actualReceivedEntry.Invoice);
                                log.WarnFormat("No ASN details found for AsnNumber: {0}, DoNumber: {1}, Invoice: {2}",
                                                   actualReceivedEntry.AsnNumber, actualReceivedEntry.DoNumber, actualReceivedEntry.Invoice);
                            }

                        }
                        else
                        {
                            _logger.LogWarning("actualReceivedEntry is null, unable to retrieve ASN details.");
                            log.Warn("actualReceivedEntry is null, unable to retrieve ASN details.");
                        }
                        //L?u l?ch s? cho ActualReceived
                        await service.AddAllActualToHistoryAsync(actualReceivedEntry.ActualReceivedId);
                    }
                }

                //N?u d? li?u ?? t?n t?i trong DB th? ki?m tra xem d? li?u m?i c? ReceiveStatus = true ko (??i v?i c?c tr??ng h?p ko c? DoNumber & Asn m? tr?ng Invoice)
                else if (exists != null && !nextItem.ReceiveStatus)
                {

                    if (!exists.IsCompleted)
                    {
                        await service.UpdateActualReceivedCompletionAsync(exists.ActualReceivedId, true);
                    }
                    await _hubConnection.SendAsync("ErrorReceived", exists.ActualReceivedId, exists.SupplierCodeNavigation.SupplierName, exists.IsCompleted);

                }

                //N?u d? li?u ?? t?n t?i trong DB th? ki?m tra xem d? li?u m?i c? ReceiveStatus = true ko (??i v?i c?c tr??ng h?p ko c? DoNumber & Asn m? tr?ng Invoice)
                else if (exists != null && nextItem.IsCompleted)
                {
                    //var data = (await service.GetActualReceivedAsyncByInfor(nextItem.AsnNumber, nextItem.DoNumber, nextItem.Invoice)).FirstOrDefault();
                    if (!exists.IsCompleted)
                    {
                        await service.UpdateActualReceivedCompletionAsync(exists.ActualReceivedId, true);

                        //X?a c?c actualDetail tr??c khi x? l? v? c?p nh?t l?i actualDetail sau khi x? l?
                        //(T?i v? ? th?i ?i?m receive status ???c c?p nh?t th?nh true th? kh?ng c? d? li?u tr? v?
                        //& d? li?u m?i ???c c?p nh?t l? d? li?u ?? ???c chia pallet r?i + ?? ??m b?o lo?i b? c?c m? h?ng chuy?n v? kho trong )
                        await service.DeleteActualDetailsByReceivedIdAsync(exists.ActualReceivedId);

                        //var asnDetails = await ParseAsnDetailFromFile(
                        /****
                        ***
                        **
                        */
                        var asnDetails = await service.GetAsnDetailAsync(
                        exists.AsnNumber,
                        exists.DoNumber,
                        exists.Invoice);

                        if (asnDetails != null && asnDetails.Any())
                        {
                            foreach (var asnDetail in asnDetails)
                            {
                                var actualDetail = new Actualdetailtlip
                                {
                                    ActualReceivedId = exists.ActualReceivedId,
                                    PartNo = asnDetail.PartNo,
                                    Quantity = asnDetail.Quantity,
                                    QuantityRemain = asnDetail.QuantityRemain,
                                    QuantityScan = asnDetail.QuantityScan,
                                    StockInStatus = asnDetail.StockInStatus,
                                    StockInLocation = asnDetail.StockInLocation,
                                };

                                await service.AddActualDetailAsync(actualDetail);
                            }
                        }


                        var actualReceivedChange = await service.GetAllActualReceivedAsyncById(exists.ActualReceivedId);
                        var actualReceivedParse = actualReceivedChange.FirstOrDefault();
                        var actualReceivedDTO = service.MapToActualReceivedTLIPDTO(actualReceivedParse);

                        await _hubConnection.SendAsync("UpdateColorScanDone", actualReceivedDTO);

                        // Sau khi ?? c?p nh?t ???c IsCompleted = true th? c?p nh?t th?m event ?? hi?n th? storage
                        //await _hubContext.Clients.All.SendAsync("UpdateStorageCalendar", actualReceivedDTO);

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
                await service.UpdateActualLeadTime(actualReceived, DateTime.Now);

                try
                {
                    await _hubConnection.SendAsync("UpdateLeadtime", service.MapToActualReceivedTLIPDTO(actualReceived));
                    _logger.LogInformation("Successfully sent lead time update to SignalR hub.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send lead time update to SignalR hub.");
                }

                //L?y d? li?u asnDetail t? API
                var asnDetails = await service.GetAsnDetailAsync(
                    /****
                    ***
                    **
                    */
                    //var asnDetails = await ParseAsnDetailFromFile(
                    actualReceived.AsnNumber ?? string.Empty,
                    actualReceived.DoNumber ?? string.Empty,
                    actualReceived.Invoice ?? string.Empty);

                //L?y d? li?u asnDetail t? DB
                var asnDetailsInDataBase = await service.GetAsnDetailInDataBaseAsync(
                            actualReceived.AsnNumber ?? string.Empty,
                            actualReceived.DoNumber ?? string.Empty,
                            actualReceived.Invoice ?? string.Empty);

                if (asnDetails != null && asnDetails.Any())
                {
                    foreach (var asnDetail in asnDetails)
                    {
                        ////////////////////////////////////////////////////////////////////
                        var matchingDetailInDatabase = asnDetailsInDataBase
                            .SelectMany(dbAd => dbAd.Actualdetailtlips)
                            .FirstOrDefault(dbDetail => dbDetail.PartNo == asnDetail.PartNo);

                        /////////////////////////////////////////////////////////////////////
                        if (asnDetail.QuantityRemain == 0 && matchingDetailInDatabase?.QuantityRemain != 0)
                        {
                            await service.UpdateActualDetailTLIPAsync(asnDetail.PartNo, actualReceived.ActualReceivedId, 0, null);
                        }

                        /////////////////////////////////////////////////////////////////
                        if ((asnDetail.QuantityScan == asnDetail.Quantity || asnDetail.QuantityScan == asnDetail.Quantity - 1)
                            && matchingDetailInDatabase?.QuantityScan != asnDetail.QuantityScan)
                        {
                            await service.UpdateActualDetailTLIPAsync(asnDetail.PartNo, actualReceived.ActualReceivedId, null, asnDetail.QuantityScan);

                            var actualReceivedChange = await service.GetAllActualReceivedAsyncById(actualReceived.ActualReceivedId);
                            var actualReceivedParse = actualReceivedChange.FirstOrDefault();
                            if (actualReceivedParse != null)
                            {
                                await _hubConnection.SendAsync("UpdatePercentage", service.MapToActualReceivedTLIPDTO(actualReceivedParse));
                            }
                        }


                    }
                }
            }
        }

        private async Task FetchStorageDetail(ISchedulereceivedTLIPService service)
        {

            var actualReceivedList = await service.GetUnstoredActualReceived();

            foreach (var actualReceived in actualReceivedList)
            {
                await service.UpdateStorageTime(actualReceived, DateTime.Now);
                //await _hubContext.Clients.All.SendAsync("UpdateStorageTime", service.MapToActualReceivedTLIPDTO(actualReceived));


                //L?y d? li?u asnDetail t? API
                var asnDetails = await service.GetAsnDetailAsync(
                    /****
                    ***
                    **
                    */
                    //var asnDetails = await ParseAsnDetailFromFile(
                    actualReceived.AsnNumber ?? string.Empty,
                    actualReceived.DoNumber ?? string.Empty,
                    actualReceived.Invoice ?? string.Empty);

                // L?y d? li?u asnDetail t? DB
                var asnDetailsInDataBase = await service.GetAsnDetailInDataBaseAsync(
                    actualReceived.AsnNumber ?? string.Empty,
                    actualReceived.DoNumber ?? string.Empty,
                    actualReceived.Invoice ?? string.Empty);

                if (asnDetails != null && asnDetails.Any())
                {
                    foreach (var asnDetail in asnDetails)
                    {
                        var matchingDetailInDatabase = asnDetailsInDataBase
                            .SelectMany(dbAd => dbAd.Actualdetailtlips)
                            .FirstOrDefault(dbDetail => dbDetail.PartNo == asnDetail.PartNo
                                                        && dbDetail.Quantity == asnDetail.Quantity
                                                        && dbDetail.QuantityScan == asnDetail.QuantityScan
                                                        && dbDetail.QuantityRemain == asnDetail.QuantityRemain
                                                        && dbDetail.ActualReceivedId == actualReceived.ActualReceivedId);


                        if (asnDetail.StockInStatus == true && matchingDetailInDatabase?.StockInStatus == false &&
                           !string.IsNullOrEmpty(asnDetail.StockInLocation) &&
                           string.IsNullOrEmpty(matchingDetailInDatabase?.StockInLocation))
                        {

                            await service.UpdateActualDetailReceivedAsync(asnDetail.PartNo, asnDetail.Quantity, asnDetail.QuantityRemain,
                                                                           asnDetail.QuantityScan, actualReceived.ActualReceivedId,
                                                                           asnDetail.StockInStatus, asnDetail.StockInLocation);

                            // Ki?m tra xem t?t c? c?c record trong DB c? trong API c? StockInLocation != null kh?ng (DB)
                            /*bool allMatchingAsnDetailsInDataBaseIsStockIn = asnDetailsInDataBase
                               .SelectMany(dbAd => dbAd.Actualdetailtlips)
                               .Where(dbDetail => asnDetails.Any(ad => ad.PartNo == dbDetail.PartNo))
                               .All(dbDetail => !string.IsNullOrEmpty(dbDetail.StockInLocation) || dbDetail.StockInStatus == true);

                            // Ki?m tra xem t?t c? c?c record trong asnDetails c? StockInLocation != null kh?ng (API)
                            bool allAsnDetailsIsStockIn = asnDetails.All(ad => !string.IsNullOrEmpty(ad.StockInLocation) || ad.StockInStatus == true);

                            if (allMatchingAsnDetailsInDataBaseIsStockIn && allAsnDetailsIsStockIn)
                            {
                                var newUpdateReceived = asnDetailsInDataBase
                             .SelectMany(dbAd => dbAd.Actualdetailtlips)
                             .FirstOrDefault(dbDetail => dbDetail.PartNo == asnDetail.PartNo
                                                         && dbDetail.Quantity == asnDetail.Quantity
                                                         && dbDetail.QuantityScan == asnDetail.QuantityScan
                                                         && dbDetail.QuantityRemain == asnDetail.QuantityRemain
                                                         && dbDetail.ActualReceivedId == actualReceived.ActualReceivedId);

                                await _hubContext.Clients.All.SendAsync("UpdateStorageColorDone", service.MapToActualReceivedTLIPDTO(actualReceived));

                            }*/
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
                            QuantityScan = element.GetProperty("quantityScan").GetInt32(),
                            StockInStatus = element.GetProperty("stockInStatus").GetBoolean(),
                            StockInLocation = element.GetProperty("stockInLocation").GetString()
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

            try
            {
                await _hubConnection.SendAsync("UpdateCalendar", actualReceivedDTO);
                _logger.LogInformation("Successfully sent UPDATE CALENDAR update to SignalR hub.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send lead time update to SignalR hub.");
            }
        }

    }
}
