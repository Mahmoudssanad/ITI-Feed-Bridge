using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    public class RegistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
