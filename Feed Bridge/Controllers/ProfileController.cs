using Feed_Bridge.Models.DTOs;
using Feed_Bridge.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var profile = new UserProfileDto
            {
                ImgUrl = user.ImgUrl,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            return Ok(profile);
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.ImgUrl = model.ImgUrl;
            user.BirthDate = model.BirthDate;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Profile updated successfully");
        }

        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Account deleted successfully");
        }
    }
}
