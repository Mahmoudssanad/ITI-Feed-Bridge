using Feed_Bridge.IServices;
using Feed_Bridge.Models.Entities;
using Feed_Bridge.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Feed_Bridge.Controllers
{
    [Authorize]
    public class DonationController : Controller
    {
        private readonly IDonationService _donationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly INotificationService _notificationService;

        public DonationController(IDonationService donationService,
            UserManager<ApplicationUser> userManager,IWebHostEnvironment webHostEnvironment, IProductService productService, INotificationService notificationService)
        {
            _donationService = donationService;
            _userManager = userManager;
            _env = webHostEnvironment;
            _productService = productService;
            _notificationService = notificationService;
        }

        //[Authorize(Roles ="Admin")]
        [HttpGet] // for the admin to display all donations
        public async Task<IActionResult> GetAll()
        {
            ViewData["ActivePage"] = "Donors";
            var donations = await _donationService.GetAllDonations();
            return View(donations);
        }


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
                Description = model.Description,
            };
            await _donationService.Add(donation, user.Id);

            var product = new Product
            {
                Name = donation.Name,
                ImgURL = donation.ImgURL,
                ExpirDate = donation.ExpirDate,
                Quantity = donation.Quantity,
                DonationId = donation.Id, // عشان نعرف إن المنتج ده مرتبط بتبرع
            };
            await _productService.AddAsync(product);
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            foreach (var admin in admins)
            {
                await _notificationService.AddNotificationAsync(new Notification
                {
                    Title = "تبرع جديد",
                    Description = $"{user.UserName} تبرع بمنتج {donation.Name}",
                    RedirectUrl = Url.Action("Donate", "Admin"),
                    UserId = admin.Id
                });
            }

            var deliveries = await _userManager.GetUsersInRoleAsync("Delivery");
            foreach (var delivery in deliveries)
            {
                await _notificationService.AddNotificationAsync(new Notification
                {
                    Title = "تبرع جديد للتوصيل",
                    Description = $"{user.UserName} تبرع بمنتج {donation.Name}",
                    RedirectUrl = Url.Action("Donations", "Delivery"),
                    UserId = delivery.Id
                });
            }

            TempData["SuccessMessage"] = "تمت التبرع بنجاح";

            return RedirectToAction("Create");
        } //view Done

        //[Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _donationService.DeleteDonation(id);
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // جلب المنتج
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            // جلب التبرع المرتبط بالمنتج
            var donation = product.Donation;
            if (donation == null)
                return BadRequest("هذا المنتج غير مرتبط بتبرع");

            // تحويل البيانات ل ViewModel
            var viewModel = new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                ExpirDate = product.ExpirDate,
                Quantity = product.Quantity,
                Address = donation.Address,
                Phone = donation.Phone,
                Description = donation.Description,
                ExistingImageUrl = product.ImgURL
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var product = await _productService.GetByIdAsync(model.Id);
            if (product == null) return NotFound();

            var donation = product.Donation;
            if (donation == null)
                return BadRequest("هذا المنتج غير مرتبط بتبرع");

            // تحديث بيانات المنتج
            product.Name = model.Name;
            product.ExpirDate = model.ExpirDate;
            product.Quantity = model.Quantity;

            // تحديث بيانات التبرع
            donation.Address = model.Address;
            donation.Phone = model.Phone;
            donation.Description = model.Description;

            // لو المستخدم رفع صورة جديدة
            if (model.Image != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                product.ImgURL = uniqueFileName;
            }

            // حفظ التعديلات
            await _productService.UpdateAsync(product);

            TempData["SuccessMessage"] = "تم تعديل المنتج بنجاح ✅";
            return RedirectToAction("Index");
        }
        



    }
}
