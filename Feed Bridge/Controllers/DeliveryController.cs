using Feed_Bridge.IServices;
using Feed_Bridge.Models.Data;
using Feed_Bridge.Models.Entities;
using Feed_Bridge.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feed_Bridge.Controllers
{
    [Authorize(Roles = "Delivery")]
    public class DeliveryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDonationService _donationService;

        public DeliveryController(AppDbContext context, IDonationService donationService)
        {
            _context = context;
            _donationService = donationService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var totalOrders = await _context.Orders
                .Where(o => o.Status == OrderStatus.Processing)
                .CountAsync();

            var completedOrders = await _context.Orders
                .Where(o => o.Status == OrderStatus.Completed)
                .CountAsync();

            var totalDonations = await _donationService.GetAllDonations();

            ViewData["TotalOrders"] = totalOrders;
            ViewData["CompletedOrders"] = completedOrders;
            ViewData["TotalDonations"] = totalDonations.Count();

            ViewData["ActivePage"] = "Dashboard";
            return View();
        }

        // صفحة الطلبات للتوصيل
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .Where(x => !x.User.IsDeleted)
                //.Where(o => o.Status == OrderStatus.Processing || o.Status == OrderStatus.Completed)
                .ToListAsync();

            ViewData["ActivePage"] = "Orders";
            return View(orders);
        }


        // صفحة التبرعات للعرض
        [HttpGet]
        public async Task<IActionResult> Donations()
        {
            var donations = await _donationService.GetAllDonations();
            ViewData["ActivePage"] = "Donations";
            return View(donations); 
        }
    }
}
