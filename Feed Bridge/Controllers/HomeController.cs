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

       
        private readonly IReviewService _reviewService;

        public HomeController(ILogger<HomeController> logger, IReviewService reviewService)
        {
            _logger = logger;
            _reviewService = reviewService;
        }

        

        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetAllAsync();
            return View(reviews); 
        }
       
        [HttpPost]
        [Authorize]
       
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(string FeedBackMsg, int StarsNumber)
        {
            var review = new Review
            {
                FeedBackMsg = FeedBackMsg,
                StarsNumber = StarsNumber,
                UserID = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CreatedAt = DateTime.Now
            };
            await _reviewService.AddAsync(review);
            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
