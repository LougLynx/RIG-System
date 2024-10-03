using Manage_Receive_Issues_Goods.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Receive_Issues_Goods.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanDetailsReceiveDensoController : ControllerBase
    {
        private readonly IScheduleRITDService _scheduleRITDService;

        public PlanDetailsReceiveDensoController(IScheduleRITDService scheduleRITDService)
        {
            _scheduleRITDService = scheduleRITDService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlanDetails()
        {
            var planDetails = await _scheduleRITDService.GetAllPlanDetailsAsync();
            Console.WriteLine("Plan detail controller:" + planDetails);
            return Ok(planDetails);
        }
    }
}
