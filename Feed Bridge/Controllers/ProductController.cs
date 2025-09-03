using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
