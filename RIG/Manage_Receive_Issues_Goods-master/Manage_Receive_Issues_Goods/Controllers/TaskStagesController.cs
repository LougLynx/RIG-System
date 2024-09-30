        using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    namespace Manage_Receive_Issues_Goods.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class TaskStagesController : ControllerBase
        {
            // GET: api/stages/{eventId}
           // [HttpGet("{eventId}")]
            [HttpGet("1")]

        public IActionResult GetStagesForEvent(int eventId)
            {
                // Fake data for demonstration, you should replace this with actual data fetching logic from the database
                var stages = new List<object>
            {
                new { name = "Gửi chỉ thị", startTime = "08:00", endTime = "08:20", status = "Hoàn thành", assignedUser = "User A" },
                new { name = "Thu thập", startTime = "08:20", endTime = "08:40", status = "Đang chờ", assignedUser = "User B" },
                new { name = "Tạo CML", startTime = "08:40", endTime = "09:00", status = "Loading...", assignedUser = "User C" }
            };

                return Ok(stages);
            }
        }
    }
    