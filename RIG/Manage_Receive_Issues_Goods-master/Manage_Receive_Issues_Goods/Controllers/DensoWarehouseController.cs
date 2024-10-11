using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Service;
using Manage_Receive_Issues_Goods.Service.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Manage_Receive_Issues_Goods.Controllers
{
	public class DensoWarehouseController : Controller
	{
		private readonly IScheduleRITDService _schedulereceivedService;

		public DensoWarehouseController(IScheduleRITDService schedulereceivedService)
		{
			_schedulereceivedService = schedulereceivedService;
		}
		public async Task<IActionResult> ScheduleReceive()
		{
			var planDetails = await _schedulereceivedService.GetPlanDetailsForDisplayAsync();
			var (currentPlan, nextPlan) = await _schedulereceivedService.GetCurrentAndNextPlanAsync();
            var pastPlanDetails = await _schedulereceivedService.GetPastPlanDetailsAsync();

            ViewData["PlanDetailsJson"] = JsonSerializer.Serialize(planDetails);
			ViewData["CurrentPlanEffectiveDate"] = currentPlan?.EffectiveDate.ToString("yyyy-MM-dd");
			ViewData["NextPlanEffectiveDate"] = nextPlan?.EffectiveDate.ToString("yyyy-MM-dd") ?? string.Empty; // Trả về chuỗi rỗng nếu không có nextPlan
            ViewData["PastPlanDetailsJson"] = JsonSerializer.Serialize(pastPlanDetails);
            return View(planDetails);
		}

		[HttpGet]
		public async Task<IActionResult> GetUpdatedEvents()
		{
			var planDetails = await _schedulereceivedService.GetPlanDetailsForDisplayAsync();
			var (currentPlan, nextPlan) = await _schedulereceivedService.GetCurrentAndNextPlanAsync();
			var pastPlanDetails = await _schedulereceivedService.GetPastPlanDetailsAsync();

			// Filter actuals for the current day
			var today = DateTime.Today;
			var filteredPlanDetails = planDetails.Select(detail => new PlanDetailDTO
			{
				PlanDetailId = detail.PlanDetailId,
				PlanTime = detail.PlanTime,
				PlanDetailName = detail.PlanDetailName,
				Actuals = detail.Actuals?.Where(actual => actual.ActualTime.Date == today).ToList()
			}).ToList();

			var result = new
			{
				planDetails = filteredPlanDetails,
				currentPlanEffectiveDate = currentPlan?.EffectiveDate.ToString("yyyy-MM-dd"),
				nextPlanEffectiveDate = nextPlan?.EffectiveDate.ToString("yyyy-MM-dd") ?? string.Empty,
				pastPlanDetails
			};

			var options = new JsonSerializerOptions
			{
				PropertyNamingPolicy = null // Use null to preserve original property names (PascalCase)
			};

			return new JsonResult(result, options);
		}


		[HttpPost]
		public async Task<IActionResult> AddActual([FromBody] ActualDetailDTO actualDetailDto)
		{
			if (actualDetailDto == null || actualDetailDto.PlanDetailId <= 0)
			{
				return Json(new { success = false, message = "Invalid data" });
			}

			try
			{
				// Kiểm tra và chuyển đổi ActualTime nếu cần thiết
				if (actualDetailDto.ActualTime == DateTime.MinValue)
				{
					return Json(new { success = false, message = "Invalid ActualTime" });
				}

				var newActual = new Actualsritd
				{
					PlanDetailId = actualDetailDto.PlanDetailId,
					ActualTime = actualDetailDto.ActualTime
				};

				// Lưu actual vào database
				await _schedulereceivedService.AddActualAsync(newActual);

				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteActual(int id)
		{
			if (id <= 0)
			{
				return Json(new { success = false, message = "Invalid Actual ID" });
			}

			try
			{
				await _schedulereceivedService.DeleteActualAsync(id);
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

      

        public IActionResult ScheduleIssued()
		{
			return View();
		}

	}
}
