using Feed_Bridge.IServices;
using Feed_Bridge.Models.Data;
using Feed_Bridge.Models.Entities;
using Feed_Bridge.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;


        public AdminController(AppDbContext context,IDonationService donationService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _donationService = donationService;
            _userManager = userManager;
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
        [HttpGet]
        public async Task<IActionResult> GetAllSupports()
        {
            var supports = await _context.Supports
                .Include(s => s.User)
                .ToListAsync();
            return View(supports);
        }
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
            var users = _userManager.Users.ToList();

            var userList = new List<UserWithRoleVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserWithRoleVM
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    ImgUrl = user.ImgUrl,
                    Roles = roles
                });
            }

            return View(userList);
        }

        // All Partners
        [HttpGet]
        public async Task<IActionResult> AllPartners()
        {
            
            var partners = await _context.Parteners.ToListAsync();
            return View(partners);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            // Remove old roles
            await _userManager.RemoveFromRolesAsync(user, roles);

            // Add new role
            await _userManager.AddToRoleAsync(user, newRole);

            TempData["Success"] = $"تم تحديث دور المستخدم {user.UserName} بنجاح إلى {newRole}.";
            return RedirectToAction("AllUsers");
        }

        // Home Control
        [HttpGet]
        public IActionResult HomeControl()
        {
            return View();
        }
    }
}
