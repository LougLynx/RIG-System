using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage_Receive_Issues_Goods.Services;
using Manage_Receive_Issues_Goods.Models;

namespace Manage_Receive_Issues_Goods.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesReceivedTLIPController : ControllerBase
    {
        private readonly ISchedulereceivedTLIPService _schedulereceivedService;

        public ResourcesReceivedTLIPController(ISchedulereceivedTLIPService schedulereceivedService)
        {
            _schedulereceivedService = schedulereceivedService;
        }

        // GET: api/Resources
        [HttpGet]
        public async Task<IActionResult> GetResources([FromQuery] int? weekdayId)
        {
            var resources = new List<dynamic>();

            int currentWeekday = (int)DateTime.Now.DayOfWeek; // Lấy thứ hiện tại (0 = Sunday, 1 = Monday, ...)
            if (currentWeekday == 0) currentWeekday = 7;

            int effectiveWeekdayId = weekdayId ?? currentWeekday;

            var suppliers = await _schedulereceivedService.GetSuppliersByWeekdayAsync(effectiveWeekdayId);

            Console.WriteLine($"Weekday {effectiveWeekdayId}: {suppliers.Count()} suppliers found.");

            foreach (var supplier in suppliers)
            {
                resources.Add(new
                {
                    id = $"{supplier.SupplierCode}_Plan",
                    title = $"{supplier.SupplierName}-PLAN",
                    eventColor = "#1E2B37",
                    cssClass = ""
                });

                resources.Add(new
                {
                    id = $"{supplier.SupplierCode}_Actual",
                    title = $"{supplier.SupplierName}-ACTUAL",
                    eventColor = "#C7B44F",
                    cssClass = "gray-background"
                });
            }

            return Ok(resources);
        }

        [HttpPost("GetWeekday")]
        public IActionResult GetWeekday([FromBody] DateRequest request)
        {
            if (DateTime.TryParse(request.Date, out DateTime date))
            {
                var dayOfWeek = date.DayOfWeek;
                var weekdayMapping = new Dictionary<DayOfWeek, int>
                {
                    { DayOfWeek.Monday, 1 },
                    { DayOfWeek.Tuesday, 2 },
                    { DayOfWeek.Wednesday, 3 },
                    { DayOfWeek.Thursday, 4 },
                    { DayOfWeek.Friday, 5 },
                    { DayOfWeek.Saturday, 6 },
                    { DayOfWeek.Sunday, 7 }
                };

                if (weekdayMapping.TryGetValue(dayOfWeek, out int weekdayId))
                {
                    return Ok(new { WeekdayId = weekdayId });
                }
                else
                {
                    return BadRequest("Invalid day of week");
                }
            }
            return BadRequest("Invalid date format");
        }
    }
}
