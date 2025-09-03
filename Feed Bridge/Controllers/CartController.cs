using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
