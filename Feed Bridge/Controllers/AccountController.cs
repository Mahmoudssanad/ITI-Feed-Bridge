using Feed_Bridge.Models.DTOs;
using Feed_Bridge.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized("User not logged in");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Password changed successfully");
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound("User not found");

            // Generate reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Ok(new { Message = "Reset token generated", Token = token });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound("User not found");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Password reset successfully");
        }
    }
}
