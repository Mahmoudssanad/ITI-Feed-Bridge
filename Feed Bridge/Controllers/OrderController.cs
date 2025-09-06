using Feed_Bridge.IServices;
using Feed_Bridge.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Confirm()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var (success, message, order) = await _orderService.ConfirmOrderAsync(user.Id);

            TempData[success ? "Success" : "Error"] = message;

            if (!success)
                return RedirectToAction("Index", "Cart");

            return RedirectToAction("Details", "Order", new { id = order.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null || order.UserId != user.Id)
            {
                return NotFound(); // تأمين إن المستخدم يشوف بس طلباته
            }

            return View(order);
        }

        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var orders = await _orderService.GetUserOrdersAsync(user.Id);

            return View(orders);
        }
    }
}
