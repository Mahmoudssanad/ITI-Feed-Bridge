using Feed_Bridge.IServices;
using Feed_Bridge.Models.Entities;
using Feed_Bridge.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Feed_Bridge.Controllers
{
    public class DonationController : Controller
    {
        private readonly IDonationService _donationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public DonationController(IDonationService donationService, UserManager<ApplicationUser> userManager,IWebHostEnvironment webHostEnvironment )
        {
            _donationService = donationService;
            _userManager = userManager;
            _env = webHostEnvironment;
        }
        //[Authorize(Roles ="Admin")]
        [HttpGet] // for the admin to display all donations
        public async Task<IActionResult> GetAll()
        {
            var donations = await _donationService.GetAllDonations();
            return View(donations);
        } //view Done
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Donation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // معالجة رفع الصورة
            string? fileName = null;
            if (model.Image != null)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                string path = Path.Combine(_env.WebRootPath, "uploads", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }

            // تحويل ViewModel → Entity
            var donation = new Donation
            {
                Name = model.Name,
                ImgURL = fileName,
                ExpirDate = model.ExpirDate,
                Quantity = model.Quantity,
                Address = model.Address,
                Phone = model.Phone,
                Description = model.Description
            };

            await _donationService.Add(donation, user.Id);

            return RedirectToAction("Index","Home");
        } //view Done

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _donationService.DeleteDonation(id);
            return RedirectToAction("GetAll");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var donation = await _donationService.GetDonationById(id);
            if (donation == null) return NotFound();

            return View(donation);
        }//view not done

        [HttpPost]
        public async Task<IActionResult> Edit(Donation donation)
        {
            if (!ModelState.IsValid) return View(donation);

            await _donationService.UpdateDonation(donation);
            return RedirectToAction("GetAll");
        }// View not done


    }
}
