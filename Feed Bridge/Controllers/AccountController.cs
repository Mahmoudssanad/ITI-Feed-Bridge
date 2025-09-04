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

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var found = await _userManager.FindByEmailAsync(model.Email);

                if (found == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = model.Name,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        BirthDate = model.BirthDate,
                        PasswordHash = model.Password
                    };

                    // Save the new user in database with hashed password
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // ممكن تسجله دخول مباشرة
                        await _signInManager.SignInAsync(user, model.RememberMe);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // لو فيه أخطاء من Identity (زي الباسورد ضعيف)
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                }
            }

            return View(model);
        }

    }
}
