using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Hubs;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        public TLIPWarehouseController(ISchedulereceivedTLIPService schedulereceivedService, IHubContext<UpdateReceiveTLIPHub> hubContext, ILogger<TLIPWarehouseController> logger, RigContext context)
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
					Invoice = actualReceivedWithSupplier.Invoice
				};

				// Notify clients via SignalR
				await _hubContext.Clients.All.SendAsync("UpdateCalendar", actualReceivedDTO);
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
                ActualDetails = ar.Actualdetailtlips.Select(detail => new ActualDetailTLIPDTO
                {
                    ActualDetailId = detail.ActualDetailId,
                    PartNo = detail.PartNo,
                    Quantity = detail.Quantity ?? 0,
                    QuantityRemain = detail.QuantityRemain ?? 0
                }).ToList()
            }).ToList();

            return Json(actualReceivedDTOList);
        }



        [HttpGet]
        public async Task<IActionResult> GetActualReceivedEntry(string supplierCode, DateTime actualDeliveryTime)
        {
			var actualReceivedEntry = await _schedulereceivedService.GetActualReceivedEntryAsync(supplierCode, actualDeliveryTime);

			if (actualReceivedEntry == null)
            {
                return NotFound();
            }

            var actualReceivedDTO = new ActualReceivedTLIPDTO
            {
                ActualReceivedId = actualReceivedEntry.ActualReceivedId,
                ActualDeliveryTime = actualReceivedEntry.ActualDeliveryTime,
                ActualLeadTime = actualReceivedEntry.ActualLeadTime,
                SupplierCode = actualReceivedEntry.SupplierCode,
                DoNumber = actualReceivedEntry.DoNumber,
                Invoice = actualReceivedEntry.Invoice,
                SupplierName = actualReceivedEntry.SupplierCodeNavigation.SupplierName,
                ActualDetails = actualReceivedEntry.Actualdetailtlips.Select(detail => new ActualDetailTLIPDTO
                {
                    ActualDetailId = detail.ActualDetailId,
                    PartNo = detail.PartNo,
                    ActualReceivedId = detail.ActualReceivedId
                }).ToList()
            };

            return Ok(actualReceivedDTO);
        }


        [HttpGet]
        public async Task<IActionResult> ParseAsnInformationFromFile()
        {
            var asnInformationList = await ParseAsnInformationFromFileAsync();
            return Json(asnInformationList);
        }




        public async Task<List<AsnInformation>> ParseAsnInformationFromFileAsync()
        {
			//string filePath = @"D:\Project Stock Delivery\RIG\RIG\demoTLIP.txt";
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
        }

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



        [HttpGet]
        public async Task<IActionResult> ParseAsnDetailFromFile(string asnNumber, string doNumber, string invoice)
        {
            var asnInformationList = await ParseAsnDetailFromFileAsync(asnNumber, doNumber, invoice);
            return Json(asnInformationList);
        }



		public async Task<List<AsnDetailData>> ParseAsnDetailFromFileAsync(string asnNumber, string doNumber, string invoice)
		{
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
									 (!string.IsNullOrEmpty(doNumber) && element.GetProperty("doNumber").GetString() == doNumber) ||
									 (!string.IsNullOrEmpty(invoice) && element.GetProperty("invoice").GetString() == invoice);

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



    }
}
