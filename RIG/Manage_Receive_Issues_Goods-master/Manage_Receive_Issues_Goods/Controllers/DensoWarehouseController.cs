using Manage_Receive_Issues_Goods.DTO;
using Manage_Receive_Issues_Goods.Models;
using Manage_Receive_Issues_Goods.Service;
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

        /*public async Task<IActionResult> ScheduleReceive()
		{
			var planDetails = await _schedulereceivedService.GetAllPlanDetailsAsync();
			var actualDetails = await _schedulereceivedService.GetAllActualsAsync();

			// Ánh xạ từ thực thể sang DTO
			var planDetailsDto = planDetails.Select(plan => new PlanDetailDTO
			{
				PlanDetailId = plan.PlanDetailId,
				PlanDetailName = plan.PlanDetailName,
				PlanTime = plan.PlanTime,
				Actuals = actualDetails.Where(a => a.PlanDetailId == plan.PlanDetailId)
									   .Select(a => new ActualDetailDTO
									   {
										   ActualId = a.ActualId,
										   ActualTime = a.ActualTime
									   }).ToList()
			}).ToList();

			// Chuyển đổi thành JSON và truyền vào ViewData
			var planDetailsJson = JsonSerializer.Serialize(planDetailsDto);
			ViewData["PlanDetailsJson"] = planDetailsJson;

			return View(planDetailsDto);
		}*/
        /*public async Task<IActionResult> ScheduleReceive()
        {
            var planDetails = await _schedulereceivedService.GetPlanAndActualDetailsAsync();

            // Chuyển đổi thành JSON và truyền vào ViewData
            var planDetailsJson = JsonSerializer.Serialize(planDetails);
            ViewData["PlanDetailsJson"] = planDetailsJson;

            return View(planDetails);
        }*/
        public async Task<IActionResult> ScheduleReceive()
        {
            var planDetails = await _schedulereceivedService.GetPlanDetailsForDisplayAsync();
            ViewData["PlanDetailsJson"] = JsonSerializer.Serialize(planDetails);
            return View(planDetails);
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

        [HttpDelete]
        public async Task<IActionResult> DeleteOldActuals()
        {
            await _schedulereceivedService.DeleteOldActualsAsync();
            return NoContent();
        }

        public IActionResult ScheduleIssued()
		{
			return View();
		}

	}
}
