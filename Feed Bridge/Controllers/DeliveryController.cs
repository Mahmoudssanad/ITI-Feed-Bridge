using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    [Authorize(Roles = "Delivery")]
    public class DeliveryController : Controller
    {
        [HttpGet]
        public IActionResult Delivery()
        {
            return View();
        }
    }
}
