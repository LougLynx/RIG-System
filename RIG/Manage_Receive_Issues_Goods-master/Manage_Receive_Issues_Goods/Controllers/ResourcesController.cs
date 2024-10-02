using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Receive_Issues_Goods.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        // GET: api/Resources
        [HttpGet]
        public IActionResult GetResources()
        {
            var resources = new List<dynamic>
    {
                //Lẻ là Plan, Chẵn là Actual
        new { id = "1", title = "Plan", eventColor = "#1E2B37", order = 1, cssClass = "" },
        new { id = "2", title = "Actual", eventColor = "#3E7D3E  ", order = 2, cssClass = "gray-background" },
        new { id = "3", title = "Plan", eventColor = "#1E2B37", order = 3, cssClass = "" },
        new { id = "4", title = "Actual", eventColor = "#3E7D3E  ", order = 4, cssClass = "gray-background" },
        new { id = "5", title = "Plan", eventColor = "#1E2B37", order = 5, cssClass = "" },
        new { id = "6", title = "Actual", eventColor = "#3E7D3E  ", order = 6, cssClass = "gray-background" },
        new { id = "7", title = "Plan", eventColor = "#1E2B37", order = 7, cssClass = "" },
        new { id = "8", title = "Actual", eventColor = "#3E7D3E  ", order = 8, cssClass = "gray-background" },
        new { id = "9", title = "Plan", eventColor = "#1E2B37", order = 9, cssClass = "" },
        new { id = "10", title = "Actual", eventColor = "#3E7D3E  ", order = 10, cssClass = "gray-background" },
        new { id = "11", title = "Plan", eventColor = "#1E2B37", order = 11, cssClass = "" },
        new { id = "12", title = "Actual", eventColor = "#3E7D3E  ", order = 12, cssClass = "gray-background" },
        new { id = "13", title = "Plan", eventColor = "#1E2B37", order = 13, cssClass = "" },
        new { id = "14", title = "Actual", eventColor = "#3E7D3E  ", order = 14, cssClass = "gray-background" },
        new { id = "15", title = "Plan", eventColor = "#1E2B37", order = 15, cssClass = "" },
        new { id = "16", title = "Actual", eventColor = "#3E7D3E  ", order = 16, cssClass = "gray-background" },
        new { id = "17", title = "Plan", eventColor = "#1E2B37", order = 17, cssClass = "" },
        new { id = "18", title = "Actual", eventColor = "#3E7D3E  ", order = 18, cssClass = "gray-background" }
    };

            // Trả về đúng thứ tự Plan trước Actual 
            return Ok(resources.OrderBy(r => r.order));
        }
    }   
}

