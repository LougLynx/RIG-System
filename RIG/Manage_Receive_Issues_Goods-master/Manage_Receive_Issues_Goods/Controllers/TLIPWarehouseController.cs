using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Fomat;
using Manage_Receive_Issues_Goods.Hubs;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manage_Receive_Issues_Goods.Controllers
{
    //[Authorize]
    public class TLIPWarehouseController : Controller
    {
        private readonly ISchedulereceivedTLIPService _schedulereceivedService;
        private readonly IHubContext<UpdateReceiveTLIPHub> _hubContext;
        private readonly ILogger<TLIPWarehouseController> _logger;
        private readonly RigContext _context;
        private List<AsnInformation> previousData = new List<AsnInformation>();
        public TLIPWarehouseController(
            ISchedulereceivedTLIPService schedulereceivedService, 
            IHubContext<UpdateReceiveTLIPHub> hubContext,
            ILogger<TLIPWarehouseController> logger, 
            RigContext context)
        {
            _schedulereceivedService = schedulereceivedService;
            _hubContext = hubContext;
            _logger = logger;
            _context = context;
        }

        public IActionResult ScheduleReceive()
        {
            return View();
        }

        public IActionResult ScheduleIssued()
        {
            return View();
        }

       

        [HttpGet]
        public async Task<JsonResult> GetSuppliersForToday()
        {
            var suppliers = await _schedulereceivedService.GetSuppliersForTodayAsync();
            var supplierList = suppliers.Select(s => new { supplierName = s.SupplierName }).ToList();
            return Json(supplierList);
        }


        [HttpGet]
        public async Task<JsonResult> GetAsnInformation()
        {
            var data = await _schedulereceivedService.GetAsnInformationAsync(DateTime.Now);
            return Json(data);
        }


        [HttpGet]
        public async Task<IActionResult> GetAsnDetail(string asnNumber, string doNumber, string invoice)
        {
            var asnDetail = await _schedulereceivedService.GetAsnDetailAsync(asnNumber, doNumber, invoice);
            return Json(asnDetail);
        }




		[HttpPost]
		public async Task<IActionResult> AddActualReceived([FromBody] Actualreceivedtlip actualReceived)
		{
			if (actualReceived == null)
			{
				return BadRequest("Invalid data.");
			}

			try
			{
				await _schedulereceivedService.AddActualReceivedAsync(actualReceived);

				var actualReceivedWithSupplier = await _schedulereceivedService.GetActualReceivedWithSupplierAsync(actualReceived.ActualReceivedId);

				var actualReceivedDTO = new ActualReceivedTLIPDTO
				{
					ActualReceivedId = actualReceivedWithSupplier.ActualReceivedId,
					ActualDeliveryTime = actualReceivedWithSupplier.ActualDeliveryTime,
					ActualLeadTime = actualReceivedWithSupplier.ActualLeadTime,
					SupplierCode = actualReceivedWithSupplier.SupplierCode,
					SupplierName = actualReceivedWithSupplier.SupplierCodeNavigation?.SupplierName ?? "Unknown Supplier",
					AsnNumber = actualReceivedWithSupplier.AsnNumber,
					DoNumber = actualReceivedWithSupplier.DoNumber,
					Invoice = actualReceivedWithSupplier.Invoice,
                    IsCompleted = actualReceivedWithSupplier.IsCompleted
                };

                return Ok(actualReceivedDTO);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding ActualReceived.");
				return StatusCode(500, "Internal server error.");
			}
		}

		[HttpPost]
        public async Task<IActionResult> AddActualDetail([FromBody] Actualdetailtlip actualDetail)
        {
            if (actualDetail == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
				await _schedulereceivedService.AddActualDetailAsync(actualDetail);
				return Ok(actualDetail);
			}
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ActualDetailTLIP.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetCurrentPlanDetailsWithDates()
        {
            var planDetails = await _schedulereceivedService.GetAllCurrentPlanDetailsAsync();

            var currentYear = DateTime.Now.Year;
            var currentWeekOfYear = _schedulereceivedService.GetWeekOfYear(DateTime.Now);

            var planDetailsWithDates = planDetails.Select(planDetail =>
            {
                var specificDate = _schedulereceivedService.GetDateForWeekday(currentYear, currentWeekOfYear, planDetail.WeekdayId);

                return new PlanDetailTLIPDTO
                {
                    PlanDetailId = planDetail.PlanDetailId,
                    SupplierCode = planDetail.SupplierCode,
                    SupplierName = planDetail.SupplierCodeNavigation?.SupplierName,
                    LeadTime = planDetail.LeadTime,
                    DeliveryTime = planDetail.DeliveryTime,
                    PlanId = planDetail.PlanId,
                    WeekdayId = planDetail.WeekdayId,
                    SpecificDate = specificDate
                };
            }).ToList();

            return Json(planDetailsWithDates);
        }




        [HttpGet]
        public async Task<JsonResult> GetActualReceived()
        {
            var actualReceivedList = await _schedulereceivedService.GetAllActualReceivedAsync();
            var actualReceivedDTOList = actualReceivedList.Select(ar => new ActualReceivedTLIPDTO
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

            return Json(actualReceivedDTOList);
        }

        [HttpGet]
        public async Task<IActionResult> GetIncompleteActualReceived()
        {
            try
            {
                var actualReceivedList = await _schedulereceivedService.GetAllActualReceivedAsync();
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

                return Ok(incompleteActualReceivedDTOList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching incomplete actual received data");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        public async Task<JsonResult> GetActualReceivedById(int actualReceivedId)
        {
            var actualReceivedList = await _schedulereceivedService.GetAllActualReceivedAsyncById(actualReceivedId);
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
            }).FirstOrDefault();

            if (actualReceivedDTO == null)
            {
                return Json(new { error = "Not Found" });
            }
            await _hubContext.Clients.All.SendAsync("UpdateCalendar", actualReceivedDTO);
            return Json(actualReceivedDTO);
        }


        [HttpGet]
        public async Task<JsonResult> GetActualReceivedByInfor(string asnNumber, string doNumber, string invoice)
        {
            try
            {
                var actualReceivedList = await _schedulereceivedService.GetActualReceivedAsyncByInfor(asnNumber, doNumber, invoice);
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
                }).FirstOrDefault();
                return new JsonResult(actualReceivedDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetActualReceivedByInfor");
                return new JsonResult(new { success = false, message = "An error occurred while fetching the data." });
            }
        }



        [HttpGet]
        public async Task<JsonResult> GetAllActualReceivedLast7Days()
        {
            var actualReceivedList = await _schedulereceivedService.GetAllActualReceivedLast7DaysAsync();
            var actualReceivedDTOList = actualReceivedList.Select(ar => new ActualReceivedTLIPDTO
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

            return Json(actualReceivedDTOList);
        }

        private double CalculateCompletionPercentage(Actualreceivedtlip actualReceived)
        {
            var totalItems = actualReceived.Actualdetailtlips.Count;
            var completedItems = actualReceived.Actualdetailtlips.Count(detail => detail.QuantityRemain == 0);

            if (totalItems == 0) return 0;
            return (completedItems / (double)totalItems) * 100;
        }



        [HttpGet]
        public async Task<IActionResult> GetActualReceivedEntry(string supplierCode, DateTime actualDeliveryTime, string asnNumber)
        {
			var actualReceivedEntry = await _schedulereceivedService.GetActualReceivedEntryAsync(supplierCode, actualDeliveryTime, asnNumber);

			if (actualReceivedEntry == null)
            {
                return NotFound();
            }

            var actualReceivedDTO = new ActualReceivedTLIPDTO
            {
                ActualReceivedId = actualReceivedEntry.ActualReceivedId,
                ActualDeliveryTime = actualReceivedEntry.ActualDeliveryTime,
                ActualLeadTime = actualReceivedEntry.ActualLeadTime,
                AsnNumber = actualReceivedEntry.AsnNumber,
                SupplierCode = actualReceivedEntry.SupplierCode,
                DoNumber = actualReceivedEntry.DoNumber,
                Invoice = actualReceivedEntry.Invoice,
                SupplierName = actualReceivedEntry.SupplierCodeNavigation.SupplierName,
                IsCompleted = actualReceivedEntry.IsCompleted,
                ActualDetails = actualReceivedEntry.Actualdetailtlips.Select(detail => new ActualDetailTLIPDTO
                {
                    ActualDetailId = detail.ActualDetailId,
                    PartNo = detail.PartNo,
                    ActualReceivedId = detail.ActualReceivedId,
                    Quantity = detail.Quantity ?? 0,
                    QuantityRemain = detail.QuantityRemain ?? 0
                }).ToList()
            };

            return Ok(actualReceivedDTO);
        }


       /* [HttpGet]
        public async Task<IActionResult> ParseAsnInformationFromFile()
        {
            var asnInformationList = await ParseAsnInformationFromFileAsync();
            return Json(asnInformationList);
        }




        public async Task<List<AsnInformation>> ParseAsnInformationFromFileAsync()
        {
           // string filePath = @"D:\Project Stock Delivery\RIG\RIG\demoTLIP.txt";
			string filePath = @"F:\FU\Semester_5\PRN212\Self_Study\DS_RIG\RIG\demoTLIP.txt";
			using (var reader = new StreamReader(filePath))
            {
                var fileContent = await reader.ReadToEndAsync();

                var jsonDocument = JsonDocument.Parse(fileContent);
                var asnInformationList = new List<AsnInformation>();

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
                    if (asnInformation != null)
                    {
                        asnInformationList.Add(asnInformation);
                    }
                }
                return asnInformationList;
            }
        }*/

        [HttpPost]
        public async Task<IActionResult> UpdateActualDetailTLIP(string partNo, int actualReceivedId, int quantityRemain)
        {
            if (string.IsNullOrEmpty(partNo) || actualReceivedId <= 0)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                await _schedulereceivedService.UpdateActualDetailTLIPAsync(partNo, actualReceivedId, quantityRemain);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ActualDetailTLIP.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetActualDetailsByReceivedId(int actualReceivedId)
        {
            var actualDetails = await _schedulereceivedService.GetActualDetailsByReceivedIdAsync(actualReceivedId);
            return Json(actualDetails);
        }



        /*[HttpGet]
        public async Task<IActionResult> ParseAsnDetailFromFile(string asnNumber, string doNumber, string invoice)
        {
            var asnInformationList = await ParseAsnDetailFromFileAsync(asnNumber, doNumber, invoice);
            return Json(asnInformationList);
        }



		public async Task<List<AsnDetailData>> ParseAsnDetailFromFileAsync(string asnNumber, string doNumber, string invoice)
		{
            //string filePath = @"D:\Project Stock Delivery\RIG\RIG\demoDetailTLIP.txt";
           string filePath = @"F:\FU\Semester_5\PRN212\Self_Study\DS_RIG\RIG\demoDetailTLIP.txt"; 

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
		}*/


        [HttpPost]
        public async Task<IActionResult> UpdateActualLeadTime([FromBody] Actualreceivedtlip actualReceived)
        {
            if (actualReceived == null || actualReceived.ActualReceivedId <= 0)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                // Tìm bản ghi actualReceived dựa trên actualReceivedId
                var existingActualReceived = await _schedulereceivedService.GetActualReceivedWithSupplierAsync(actualReceived.ActualReceivedId);

                if (existingActualReceived != null)
                {
                    // Tính toán ActualLeadTime là chênh lệch giữa thời gian kết thúc và ActualDeliveryTime
                    var endDateTime = actualReceived.ActualDeliveryTime;  // Đây là thời gian kết thúc được truyền từ FullCalendar
                    var leadTime = endDateTime - existingActualReceived.ActualDeliveryTime;
                    existingActualReceived.ActualLeadTime = TimeOnly.FromTimeSpan(leadTime);

                    // Lưu cập nhật
                    await _schedulereceivedService.UpdateActualReceivedAsync(existingActualReceived);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ActualLeadTime.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetActualReceivedByDetails([FromBody] ActualReceivedTLIPDTO details)
        {
            var actualReceived = await _context.Actualreceivedtlips
                .FirstOrDefaultAsync(ar =>
                    ar.SupplierCode == details.SupplierCode &&
                    (string.IsNullOrEmpty(details.AsnNumber) || ar.AsnNumber == details.AsnNumber) &&
                    (string.IsNullOrEmpty(details.DoNumber) || ar.DoNumber == details.DoNumber) &&
                    (string.IsNullOrEmpty(details.Invoice) || ar.Invoice == details.Invoice));

            if (actualReceived == null)
            {
                return NotFound();
            }

            return Ok(actualReceived);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActualReceivedCompletion(int actualReceivedId, bool isCompleted)
        {
            var actualReceived = await _context.Actualreceivedtlips.FindAsync(actualReceivedId);
            if (actualReceived == null)
            {
                return NotFound(new { message = "ActualReceived not found." });
            }

            actualReceived.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();

            return Ok(new { message = "ActualReceived updated successfully.", actualReceived });
        }


        [HttpPost]
        public async Task<IActionResult> UpdateActualDetailsQuantityRemain(int actualReceivedId, int quantityRemain)
        {
            var actualDetails = await _context.Actualdetailtlips.Where(ad => ad.ActualReceivedId == actualReceivedId).ToListAsync();
            if (actualDetails == null || !actualDetails.Any())
            {
                return NotFound();
            }

            foreach (var detail in actualDetails)
            {
                detail.QuantityRemain = quantityRemain;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }



        /* [HttpGet]
         public async Task<IActionResult> FetchData()
         {
             try
             {
                 _logger.LogInformation("fetchData called at {Time}", DateTime.Now.ToString("T"));
                 var now = DateTime.Now;
                 var nextData = await _schedulereceivedService.GetAsnInformationAsync(now);

                 foreach (var nextItem in nextData)
                 {
                     var previousItem = previousData.FirstOrDefault(item =>
                         (item.AsnNumber != null && item.AsnNumber == nextItem.AsnNumber) ||
                         (item.AsnNumber == null && item.DoNumber != null && item.DoNumber == nextItem.DoNumber) ||
                         (item.AsnNumber == null && item.DoNumber == null && item.Invoice != null && item.Invoice == nextItem.Invoice)
                     );

                     var exists = await _schedulereceivedService.GetActualReceivedByDetailsAsync(new ActualReceivedTLIPDTO
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

                             await _schedulereceivedService.AddActualReceivedAsync(actualReceived);

                             var actualReceivedEntry = await _schedulereceivedService.GetActualReceivedEntryAsync(actualReceived.SupplierCode, actualReceived.ActualDeliveryTime, actualReceived.AsnNumber);

                             var asnDetails = await _schedulereceivedService.GetAsnDetailAsync(actualReceivedEntry.AsnNumber, actualReceivedEntry.DoNumber, actualReceivedEntry.Invoice);

                             foreach (var asnDetail in asnDetails)
                             {
                                 var actualDetail = new Actualdetailtlip
                                 {
                                     ActualReceivedId = actualReceivedEntry.ActualReceivedId,
                                     PartNo = asnDetail.PartNo,
                                     Quantity = asnDetail.Quantity,
                                     QuantityRemain = asnDetail.QuantityRemain
                                 };

                                 await _schedulereceivedService.AddActualDetailAsync(actualDetail);
                             }
                         }

                         if (previousItem != null && !previousItem.IsCompleted && nextItem.IsCompleted)
                         {
                             var actualReceived = await _schedulereceivedService.GetActualReceivedByDetailsAsync(new ActualReceivedTLIPDTO
                             {
                                 SupplierCode = previousItem.SupplierCode,
                                 AsnNumber = previousItem.AsnNumber,
                                 DoNumber = previousItem.DoNumber,
                                 Invoice = previousItem.Invoice
                             });

                             if (actualReceived != null)
                             {
                                 await _schedulereceivedService.UpdateActualReceivedCompletionAsync(actualReceived.ActualReceivedId, true);
                                 Console.WriteLine("Updated ActualReceived IsCompleted to true successfully.");
                             }
                         }
                     }
                     else
                     {
                         if (!nextItem.ReceiveStatus)
                         {
                             Console.WriteLine("Duplicate data detected in API for ActualReceivedId: " + nextItem.AsnNumber + ", " + nextItem.DoNumber + ", " + nextItem.Invoice + ", " + exists.ActualReceivedId);
                             await _schedulereceivedService.UpdateActualReceivedCompletionAsync(exists.ActualReceivedId, true);
                             Console.WriteLine("Updated ActualReceived IsCompleted to true successfully.");
                         }
                     }
                 }

                 previousData = nextData.ToList();
                 return Ok(nextData);
             }
             catch (Exception ex)
             {
                 Console.WriteLine("Error fetching data: " + ex.Message);
                 return StatusCode(500, "Internal server error");
             }
         }*/




        /*[HttpGet]
        public async Task<IActionResult> FetchDataDetail()
        {
            try
            {
                var actualReceivedList = await GetIncompleteActualReceived();
                if (actualReceivedList is OkObjectResult okResult && okResult.Value is List<ActualReceivedTLIPDTO> actualReceivedData)
                {
                    foreach (var actualReceived in actualReceivedData)
                    {
                        var asnDetails = await _schedulereceivedService.GetAsnDetailAsync(
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
                                    await _schedulereceivedService.UpdateActualDetailTLIPAsync(asnDetail.PartNo, actualReceived.ActualReceivedId, 0);
                                }
                            }
                        }
                    }
                }
                else
                {
                    _logger.LogError("Failed to fetch incomplete actual received data.");
                    return StatusCode(500, "Internal server error");
                }

                return Ok("Data processed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data detail");
                return StatusCode(500, "Internal server error");
            }
        }*/



    }
}
