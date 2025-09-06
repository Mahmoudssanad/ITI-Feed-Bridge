using Feed_Bridge.IServices;
using Feed_Bridge.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace Feed_Bridge.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDonationService _donationService;

        public AdminController(AppDbContext context,IDonationService donationService)
        {
            _context = context;
            _donationService = donationService;
        }

        // Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            //// إجمالي المتبرعين (عدد المستخدمين اللي عندهم تبرعات)
            var totalDonors = await _context.Donations
                .Select(d => d.UserId)
                .Distinct()
                .CountAsync();

            // إجمالي التبرعات (عدد التبرعات)
            var totalDonations = await _context.Donations.CountAsync();

            // إجمالي المساعدات المالية (نجمع قيمة التبرعات المالية)
            var totalSupports = await _context.Supports.SumAsync(s => (decimal?)s.Amount) ?? 0;

            // نحط الأرقام في ViewData أو ViewModel
            ViewData["TotalDonors"] = totalDonors;
            ViewData["TotalDonations"] = totalDonations;
            ViewData["TotalSupports"] = totalSupports;

            return View();
        }

        // Orders
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.User) // لو عايز تعرض بيانات المستخدم
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .ToListAsync();

            return View(orders);
        }

        // Donors
        //[HttpGet]
        //public async Task<IActionResult> Donate()
        //{
        //    var donate = await _context.Donations
        //        .Include(d => d.User)
        //        .ToListAsync();

        //    return View(donate);
        //}

        [HttpGet] // for the admin to display all donations
        public async Task<IActionResult> Donate()
        {
            ViewData["ActivePage"] = "Donors";
            var donations = await _donationService.GetAllDonations();
            return View(donations);
        } //view Done

        // Reports
        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            var reports = await _context.Reports.ToListAsync();
            return View(reports);
        }

        // Products
        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        // Delivery
        //[HttpGet]
        //public async Task<IActionResult> Delivery()
        //{
        //    var deliveries = await _context.Deliveries
        //        .Include(d => d.Order)
        //        .ToListAsync();

        //    return View(deliveries);
        //}

        // All Users
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // All Partners
        [HttpGet]
        public async Task<IActionResult> AllPartners()
        {
            
            var partners = await _context.Parteners.ToListAsync();

            
            if (partners == null || !partners.Any())
            {
                return NotFound();
            }

            return View(partners);
        }

        // Home Control
        [HttpGet]
        public IActionResult HomeControl()
        {
            return View();
        }
    }
}
