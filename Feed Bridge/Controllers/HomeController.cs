using Feed_Bridge.IServices;
using Feed_Bridge.Models;
using Feed_Bridge.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Feed_Bridge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly IReviewService _reviewService;

        public HomeController(ILogger<HomeController> logger, IReviewService reviewService)
        {
            _logger = logger;
            _reviewService = reviewService;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetAllAsync();
            return View(reviews); 
        }
       
        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> AddReview(Review model)
        {
            if (ModelState.IsValid)
            {
               
                model.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _reviewService.AddAsync(model);
                return RedirectToAction("Index");
            }

            var reviews = await _reviewService.GetAllAsync();
            return View("Index", reviews);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
