using Feed_Bridge.IServices;
using Feed_Bridge.Models.Entities;
using Feed_Bridge.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feed_Bridge.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly INotificationService _notificationService; // ✅ Injection

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            INotificationService notificationService) // ✅ Constructor injection
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var users = await _userManager.Users
                .Include(x => x.Orders)
                .Include(x => x.Supports)
                .Where(x => x.Id != currentUser.Id && x.IsDeleted != true)
                .ToListAsync();

            var userWithRoles = new List<UserWithRolesViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                userWithRoles.Add(new UserWithRolesViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Roles = roles,
                    OrdersCount = u.Orders?.Count ?? 0,
                    SupportsCount = u.Supports?.Count ?? 0
                });
            }

            return View(userWithRoles);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var model = new EditProfileViewModel
            {
                CurrentImgUrl = user.ImgUrl,
                BirthDate = user.BirthDate.ToDateTime(TimeOnly.MinValue),
                FullName = user.UserName,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            if (model.BirthDate.HasValue)
                user.BirthDate = DateOnly.FromDateTime(model.BirthDate.Value);

            user.UserName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            if (model.ImgFile != null)
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImgFile.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImgFile.CopyToAsync(stream);
                }

                user.ImgUrl = "/uploads/" + fileName;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Profile");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await _signInManager.SignOutAsync();

                user.Email = user.Email + ".deleted." + Guid.NewGuid();
                user.NormalizedEmail = user.Email.ToUpper();
                user.UserName = user.UserName + ".deleted." + Guid.NewGuid();
                user.NormalizedUserName = user.UserName.ToUpper();
                user.IsDeleted = true;
                user.DeletedBy = "User";

                await _userManager.UpdateSecurityStampAsync(user);

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("DeleteAccount");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.CurrentPassword == model.Password)
            {
                ModelState.AddModelError("", "كلمة المرور الجديدة يجب ان تكون مختلفه عن كلمه المرور القديمه");
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "كلمة المرور الجديدة وتأكيدها غير متطابقين");
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // ✅ جلب إشعارات اليوزر (تقدرِ تعرضيه في الجرس)
        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var notifications = await _notificationService.GetUserNotificationsAsync(user.Id);
            return PartialView("_NotificationsPartial", notifications);
        }
        [HttpPost]
        [Route("MarkAllNotificationsAsRead")]
        public async Task<IActionResult> MarkAllNotificationsAsRead()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await _notificationService.MarkAllAsReadAsync(userId);
            }

            return Ok();
        }

        // لجلب عداد الإشعارات غير المقروءة
        [HttpGet]
        [Route("UnreadCount")]
        public async Task<IActionResult> UnreadCount()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int count = 0;

            if (!string.IsNullOrEmpty(userId))
            {
                count = await _notificationService.GetUnreadCountAsync(userId);
            }

            return Json(count);
        }
    }
}
