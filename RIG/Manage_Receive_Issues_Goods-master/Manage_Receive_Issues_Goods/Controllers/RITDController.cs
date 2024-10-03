using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Service;
using Manage_Receive_Issues_Goods.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manage_Receive_Issues_Goods.Controllers
{
    public class RITDController : Controller
    {
        private readonly IScheduleRITDService _scheduleService;

        public RITDController(IScheduleRITDService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public async Task<IActionResult> ChangePlan()
        {
            var planDetails = await _scheduleService.GetPlanDetailsForWeekAsync();
            return View(planDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePlanTime(int detailId, string planTime)
        {
            var detail = await _scheduleService.GetPlanDetailByIdAsync(detailId);
            if (detail != null)
            {
                detail.PlanTime = TimeOnly.Parse(planTime); 
                await _scheduleService.UpdatePlanDetailAsync(detail);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Detail not found" });
        }

        public async Task<IActionResult> ScheduleReceive()
        {
            var planDetails = await _scheduleService.GetAllPlanDetailsAsync();
            return View(planDetails);
        }


        [HttpGet]
        public async Task<JsonResult> GetPlanAndActualEvents(DateTime start, DateTime end)
        {
            var plans = await _scheduleService.GetAllPlansAsync();
            var planDetails = await _scheduleService.GetAllPlanDetailsAsync();
            var actuals = await _scheduleService.GetAllActualsAsync();
            var statuses = await _scheduleService.GetAllStatusesAsync();

            var events = planDetails.SelectMany(p =>
            {
                var actual = actuals.FirstOrDefault(a => a.PlanDetailId == p.PlanDetailId);
                var status = statuses.FirstOrDefault(s => s.PlanDetailId == p.PlanDetailId);
                var planDateTime = p.PlanDate.ToDateTime(p.PlanTime);
                return new[]
                {
                        new {
                            resourceId = "1",
                            title = $"{p.PlanDetailName} - {status?.Status1}",
                            start = planDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                            end = planDateTime.AddMinutes(60).ToString("yyyy-MM-ddTHH:mm:ss") 
                        },
                        new {
                            resourceId = "2",
                            title = $"{p.PlanDetailName} - Actual",
                            start = actual?.ActualTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                            end = actual?.ActualTime.AddMinutes(60).ToString("yyyy-MM-ddTHH:mm:ss") 
                        }
                };
            }).Where(e => e.start != null).ToList();

            return Json(events);
        }
    }
}
