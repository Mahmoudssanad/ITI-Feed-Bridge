using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    public class DeliveryController : Controller
    {
        [HttpGet]
        public IActionResult Delivery()
        {
            return View();
        }

    }
}
