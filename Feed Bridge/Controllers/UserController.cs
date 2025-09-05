using Feed_Bridge.Models.Entities;
using Feed_Bridge.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: User/Profile
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View();
        }

        // POST: User/EditProfile
        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUser model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.ImgUrl = model.ImgUrl;
            user.BirthDate = model.BirthDate;

            await _userManager.UpdateAsync(user);
            return RedirectToAction("Profile");
        }

        // POST: User/Delete
        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: User/ChangePassword
        
        
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match");
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var hasPassword = await _userManager.HasPasswordAsync(user);
            IdentityResult result;

            if (hasPassword)
            {
                await _userManager.RemovePasswordAsync(user);
                result = await _userManager.AddPasswordAsync(user, model.Password);
            }
            else
            {
                result = await _userManager.AddPasswordAsync(user, model.Password);
            }

            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            ModelState.AddModelError("", "Failed to change password");
            return View(model);
        }



        

    }
}
