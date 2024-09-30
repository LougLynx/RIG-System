using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Manage_Receive_Issues_Goods.Controllers
{
    public class ReceiveGoodsController : Controller
    {
        private readonly ISchedulereceivedService _schedulereceivedService;

        public ReceiveGoodsController(ISchedulereceivedService schedulereceivedService)
        {
            _schedulereceivedService = schedulereceivedService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Schedule()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetPlanAndActualEvents(DateTime start, DateTime end)
        {
            // Lấy danh sách Plan từ ScheduleReceived
            var scheduleReceived = await _schedulereceivedService.GetAllSchedulesAsync();

            // Lấy danh sách Actual từ ActualReceived
            var actualReceived = await _schedulereceivedService.GetAllActualReceivedAsync();


            // Xử lý kết hợp Plan và Actual
            var events = scheduleReceived.SelectMany(s =>
            {
                var actual = actualReceived.FirstOrDefault(ar => ar.ScheduleId == s.ScheduleId);
                return new[]
                {
                // Plan Event
            new {
                resourceId = (s.Supplier.SupplierId * 2 - 1).ToString(),
                title = $" {s.Supplier.SupplierName}",
                start = _schedulereceivedService.GetDateForWeekday(DateTime.Now.Year, _schedulereceivedService.GetWeekOfYear(DateTime.Now), s.WeekdayId)
                        .Add(s.DeliveryTime.Time1.ToTimeSpan()).ToString("yyyy-MM-ddTHH:mm:ss"),
                end = _schedulereceivedService.GetDateForWeekday(DateTime.Now.Year, _schedulereceivedService.GetWeekOfYear(DateTime.Now), s.WeekdayId)
                        .Add(s.DeliveryTime.Time1.ToTimeSpan()).AddMinutes(s.LeadTime).ToString("yyyy-MM-ddTHH:mm:ss")
            },
                // Actual Event (nếu có)
            new {
                resourceId = (s.Supplier.SupplierId * 2).ToString(),
                title = $" {s.Supplier.SupplierName}",
                start = actual != null ? actual.ActualDeliveryTime.ToString("yyyy-MM-ddTHH:mm:ss") : null,
                end = actual != null ? actual.ActualDeliveryTime.AddMinutes(actual.ActualLeadTime).ToString("yyyy-MM-ddTHH:mm:ss") : null
            }
        };
            }).Where(e => e.start != null).ToList();

            return Json(events);
        }

        [HttpGet]
        public async Task<JsonResult> GetSuppliersForToday()
        {
            var suppliers = await _schedulereceivedService.GetSuppliersForTodayAsync();
            var supplierList = suppliers.Select(s => new { supplierName = s.SupplierName }).ToList();
            return Json(supplierList);
        }


    }
}
