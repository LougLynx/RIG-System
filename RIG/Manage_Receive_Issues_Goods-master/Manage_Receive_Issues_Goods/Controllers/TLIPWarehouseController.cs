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
                    var supplierCodeNavigation = planDetail?.SupplierCodeNavigation;

                    var deliveryTime = planDetail?.DeliveryTime ?? default(TimeOnly);
                    var historyDate = d.HistoryDate.HasValue ? d.HistoryDate.Value : default(DateOnly);
                    DateTime? specificDate = null;

                    if (d.HistoryDate.HasValue)
                    {
                        specificDate = historyDate.ToDateTime(deliveryTime);
                    }

                    return new PlanDetailTLIPDTO
                    {
                        PlanDetailId = planDetail?.PlanDetailId ?? 0,
                        PlanId = planDetail?.PlanId ?? 0,
                        SupplierCode = planDetail?.SupplierCode,
                        SupplierName = supplierCodeNavigation?.SupplierName,
                        DeliveryTime = deliveryTime,
                        WeekdayId = planDetail?.WeekdayId ?? 0,
                        LeadTime = planDetail?.LeadTime ?? default(TimeOnly),
                        PlanType = planDetail?.PlanType,
                        WeekOfMonth = planDetail?.WeekOfMonth ?? 0,
                        HistoryDate = historyDate,
                        SpecificDate = specificDate ?? default(DateTime)
                    };
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
                actualReceivedDTOList = actualReceivedList.Select(ar => new ActualReceivedTLIPDTO
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
                        QuantityScan = detail.QuantityScan ?? 0,
                        ActualReceivedId = detail.ActualReceivedId
                    }).ToList(),
                    CompletionPercentage = CalculateCompletionPercentage(ar)
                }).ToList();
            }

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
                            QuantityScan = detail.QuantityScan ?? 0,
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
                    QuantityScan = detail.QuantityScan ?? 0,
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
                var result = actualReceivedList.Select(ar => new ActualReceivedTLIPDTO
                {
                    ActualReceivedId = ar.ActualReceivedId,
                    ActualDeliveryTime = ar.ActualDeliveryTime,
                    ActualLeadTime = ar.ActualLeadTime,
                    SupplierCode = ar.SupplierCode,
                    AsnNumber = ar.AsnNumber,
                    DoNumber = ar.DoNumber,
                    Invoice = ar.Invoice,
                    SupplierName = ar.SupplierCodeNavigation?.SupplierName,
                    CompletionPercentage = CalculateCompletionPercentage(ar),
                    IsCompleted = ar.IsCompleted
                    
                });

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching actual received data for supplier {SupplierCode}", supplierCode);
                return Json(new { success = false, message = "An error occurred while fetching data." });
            }
        }


    }
}
