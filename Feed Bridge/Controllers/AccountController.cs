using Feed_Bridge.Models.Entities;
using Feed_Bridge.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToAction("GetAll", "Donations");
                    else
                        return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "البريد الإلكتروني أو كلمة المرور غير صحيحة";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "كلمة المرور وتأكيدها غير متطابقين";
                return View();
            }

            var user = new ApplicationUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                
                if (!await _roleManager.RoleExistsAsync("User"))
                    await _roleManager.CreateAsync(new IdentityRole("User"));

                
                await _userManager.AddToRoleAsync(user, "User");

                
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "من فضلك أدخل البريد الإلكتروني";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Error = "هذا البريد غير مسجل لدينا";
                return View();
            }

            // توليد رمز إعادة التعيين
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // إنشاء لينك لإعادة تعيين كلمة المرور
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            // 🔹 تحويل المستخدم مباشرة على صفحة ResetPassword
            return Redirect(resetLink);
        }


        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ViewBag.Error = "الرابط غير صالح";
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.Error = "المستخدم غير موجود";
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                ViewBag.Success = "تم تغيير كلمة المرور بنجاح";
                return RedirectToAction("Login");
            }

            ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
            return View(model);
        }

    }
}
