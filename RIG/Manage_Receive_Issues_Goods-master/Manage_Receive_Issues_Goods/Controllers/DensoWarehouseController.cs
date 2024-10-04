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

        public async Task<IActionResult> ScheduleReceive()
        {
            var planDetails = await _schedulereceivedService.GetAllPlanDetailsAsync();
            var actualDetails = await _schedulereceivedService.GetAllActualsAsync();

            var planDetailsJson = JsonSerializer.Serialize(planDetails);
            var actualDetailsJson = JsonSerializer.Serialize(actualDetails);

            ViewData["PlanDetailsJson"] = planDetailsJson;
            ViewData["ActualDetailsJson"] = actualDetailsJson;

            return View(planDetails);
        }


        public IActionResult ScheduleIssued()
        {
            return View();
        }
 
    }
}
