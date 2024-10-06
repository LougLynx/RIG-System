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
            var planDetails = await _scheduleService.GetAllPlanDetailsAsync();
            return View(planDetails);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePlan(string planName, string planType, int totalShipment, string effectiveDate, List<PlanDetailInputModel> planDetails)
        {
            try
            {

                // Log dữ liệu planDetails để kiểm tra
                Console.WriteLine("Plan Details Received:");
                foreach (var detail in planDetails)
                {
                    Console.WriteLine($"Plan Detail - Time: {detail.PlanTime}");
                }

                // Kiểm tra nếu chuỗi effectiveDate bị null hoặc rỗng
                if (string.IsNullOrEmpty(effectiveDate))
                {
                    return Json(new { success = false, message = "EffectiveDate cannot be null or empty." });
                }
                // Chuyển đổi effectiveDate từ string sang DateOnly
                DateOnly effectiveDateOnly = DateOnly.Parse(effectiveDate);

                // Lưu thông tin Plan vào bảng PlanRITD
                var newPlan = new Planritd
                {
                    PlanName = planName,
                    PlanType = planType,
                    TotalShipment = totalShipment,
                    EffectiveDate = effectiveDateOnly
                };
                Console.WriteLine(newPlan);

                await _scheduleService.AddPlanAsync(newPlan);  // Lưu kế hoạch vào bảng PlanRITD

                // Lấy PlanID của kế hoạch vừa tạo
                var planId = await _scheduleService.GetPlanIdByDetailsAsync(planName, planType, effectiveDateOnly);
                Console.WriteLine(planId);


                if (planId == 0)
                {
                    Console.WriteLine("Không có PlanID");
                    return Json(new { success = false, message = "Không tìm thấy Plan mới tạo." });
                }
                // Lưu các PlanDetail vào bảng PlanRITDDetails
                foreach (var detail in planDetails)
                {
                    // Kiểm tra nếu planTime bị null hoặc rỗng
                    if (string.IsNullOrEmpty(detail.PlanTime))
                    {
                        return Json(new { success = false, message = "PlanTime cannot be null or empty." });
                    }
                    var newDetail = new Planritddetail
                    {
                        PlanId = planId,
                        PlanTime = TimeOnly.Parse(detail.PlanTime),  // Chuyển đổi PlanTime từ string sang TimeOnly
                        PlanDetailName = $"Số {planDetails.IndexOf(detail) + 1}"  // Đặt tên cho các PlanDetail
                    };
                    await _scheduleService.AddPlanDetailAsync(newDetail);  // Lưu từng PlanDetail
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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


        /*[HttpGet]
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
        }*/
    }
}
