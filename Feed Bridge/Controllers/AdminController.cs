using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }
    }
}
