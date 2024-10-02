using Manage_Receive_Issues_Goods.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Receive_Issues_Goods.Controllers
{
    public class DensoWarehouseController : Controller
    {
        private readonly ISchedulereceivedTLIPService _schedulereceivedService;

        public DensoWarehouseController(ISchedulereceivedTLIPService schedulereceivedService)
        {
            _schedulereceivedService = schedulereceivedService;
        }

        public IActionResult ScheduleReceive()
        {
            return View();
        }

        public IActionResult ScheduleIssued()
        {
            return View();
        }
    }
}
