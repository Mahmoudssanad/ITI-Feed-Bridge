using Feed_Bridge.IServices;
using Feed_Bridge.Models.Entities;
using Feed_Bridge.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IDonationService _donationService;

        public AdminController(IDonationService donationService)
        {
            _donationService = donationService; // DI
        }
        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllDonations()
        {
            var donations = await _donationService.GetAllDonations(); // جلب كل التبرعات
            ViewData["ActivePage"] = "Donors"; // عشان sidebar highlight
            return View(donations.ToList());
        }


    }
}
