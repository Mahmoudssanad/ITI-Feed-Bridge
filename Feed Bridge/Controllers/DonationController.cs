using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    public class DonationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
