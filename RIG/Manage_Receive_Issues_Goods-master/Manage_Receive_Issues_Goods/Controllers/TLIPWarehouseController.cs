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

        public async Task<IActionResult> SupplierList()
        {
            var suppliers = await _schedulereceivedService.GetAllSuppliersAsync();
            return View(suppliers);
        }

        public async Task<IActionResult> PlanList()
        {
            var allSupplier = await _schedulereceivedService.GetAllSuppliersAsync();
            var planDetails = await _schedulereceivedService.GetAllCurrentPlanDetailsAsync();
            var planDetailsDTO = new List<PlanDetailTLIPDTO>();

            if (planDetails != null && planDetails.Any())
            {
                planDetailsDTO = planDetails.Select(planDetail =>
                {
                    return _schedulereceivedService.MapToPlanDetailTLIPDTO(planDetail); ;
                }).ToList();
            }
            ViewData["AllSuppliers"] = allSupplier;
            return View(planDetailsDTO);
        }

        [HttpGet]
        public async Task<JsonResult> GetSuppliersForToday()
        {
            var suppliers = await _schedulereceivedService.GetSuppliersForTodayAsync();
            var supplierList = suppliers.Select(s => new
            {
                supplierCode = s.SupplierCode,
                supplierName = s.SupplierName
            }).ToList();
            return Json(supplierList);
        }


        [HttpGet]
        public async Task<JsonResult> GetPlanTripCountForToday()
        {
            var suppliersWithTripCount = await _schedulereceivedService.GetSuppliersWithTripCountForTodayAsync();
            var result = suppliersWithTripCount.Select(s => new TripCountTLIPDTO
            {
                SupplierCode = s.Supplier.SupplierCode,
                SupplierName = s.Supplier.SupplierName,
                TripCount = s.TripCount
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GeActualTripCountForToday()
        {
            var suppliersWithTripCount = await _schedulereceivedService.GeActualTripCountForTodayAsync();

            return Json(suppliersWithTripCount);
        }

        [HttpGet]
        public async Task<JsonResult> GetCurrentPlanDetailsWithDates()
        {
            var planDetails = await _schedulereceivedService.GetAllCurrentPlanDetailsAsync();
            var planDetailsWithDates = new List<PlanDetailTLIPDTO>();

            if (planDetails != null && planDetails.Any())
            {
                planDetailsWithDates = planDetails.Select(planDetail =>
                {
                    var specificDate = _schedulereceivedService.GetDateForWeekday(planDetail.WeekdayId);
                    var dto = _schedulereceivedService.MapToPlanDetailTLIPDTO(planDetail);
                    dto.SpecificDate = specificDate;
                    return dto;
                }).ToList();
            }

            return Json(planDetailsWithDates);
        }


        [HttpGet]
        public async Task<JsonResult> GetPlanDetailsBySupplierCode(string supplierCode)
        {
            var today = DateTime.Today;
            var planDetails = await _schedulereceivedService.GetAllCurrentPlanDetailsBySupplierCodeAsync(supplierCode);
            var planDetailsWithDates = new List<PlanDetailTLIPDTO>();

            if (planDetails != null && planDetails.Any())
            {
                planDetailsWithDates = planDetails
                    .Where(planDetail => _schedulereceivedService.GetDateForWeekday(planDetail.WeekdayId).Date == today)
                    .Select(planDetail =>
                    {
                        var specificDate = _schedulereceivedService.GetDateForWeekday(planDetail.WeekdayId);

                        var dto = _schedulereceivedService.MapToPlanDetailTLIPDTO(planDetail);
                        dto.SpecificDate = specificDate;
                        return dto;
                    }).ToList();
            }

            return Json(planDetailsWithDates);
        }

        [HttpGet]
        public async Task<JsonResult> GetPlanActualDetailsInHistory()
        {
            var details = await _schedulereceivedService.GetPlanActualDetailsInHistoryAsync();
            var result = new List<PlanDetailTLIPDTO>();

            if (details != null && details.Any())
            {
                result = details.Select(d =>
                {
                    var planDetail = d.PlanDetail;

                    var deliveryTime = planDetail?.DeliveryTime ?? default(TimeOnly);
                    var historyDate = d.HistoryDate.HasValue ? d.HistoryDate.Value : default(DateOnly);
                    DateTime? specificDate = null;

                    if (d.HistoryDate.HasValue)
                    {
                        specificDate = historyDate.ToDateTime(deliveryTime);
                    }

                    var dto = _schedulereceivedService.MapToPlanDetailTLIPDTO(planDetail);
                    dto.HistoryDate = historyDate;
                    dto.SpecificDate = specificDate ?? default(DateTime);

                    return dto;
                }).ToList();
            }

            return Json(result);
        }



        [HttpGet]
        public async Task<JsonResult> GetActualReceived()
        {
            var actualReceivedList = await _schedulereceivedService.GetAllActualReceivedAsync();
            var actualReceivedDTOList = new List<ActualReceivedTLIPDTO>();

            if (actualReceivedList != null && actualReceivedList.Any())
            {
                actualReceivedDTOList = actualReceivedList.Select(ar =>
                {
                    return _schedulereceivedService.MapToActualReceivedTLIPDTO(ar);
                }).ToList();
            }
            return Json(actualReceivedDTOList);
        }

        [HttpGet]
        public async Task<IActionResult> GetActualDetailsByReceivedId(int actualReceivedId)
        {
            var actualDetails = await _schedulereceivedService.GetActualDetailsByReceivedIdAsync(actualReceivedId);
            return Json(actualDetails);
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

        [HttpGet]
        public async Task<JsonResult> GetActualReceivedBySupplier(string supplierCode)
        {
            if (string.IsNullOrEmpty(supplierCode))
            {
                return Json(new { success = false, message = "Supplier code is required." });
            }

            try
            {
                var actualReceivedList = await _schedulereceivedService.GetActualReceivedBySupplierForTodayAsync(supplierCode);
                var result = actualReceivedList.Select(ar => 
                {
                    return _schedulereceivedService.MapToActualReceivedTLIPDTO(ar);

                });

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching actual received data for supplier {SupplierCode}", supplierCode);
                return Json(new { success = false, message = "An error occurred while fetching data." });
            }
        }


        [HttpGet]
        public async Task<JsonResult> GetTagNameRule()
        {
            var tagnameRule = await _schedulereceivedService.GetAllTagNameRuleAsync();

            return Json(tagnameRule);
        }
    }
}
