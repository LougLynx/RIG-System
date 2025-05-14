using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Receive_Issues_Goods.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConfigController : Controller
    {
        public IActionResult Portal()
        {
            return View();
        }
    }
}