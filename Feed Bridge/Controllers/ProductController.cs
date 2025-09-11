using Feed_Bridge.IServices;
using Feed_Bridge.Models.Entities;
using Feed_Bridge.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IDonationService _donationService;

        public ProductController(IProductService productService, IDonationService donationService)
        {
            _productService = productService;
            _donationService = donationService;
        }

        // 🟢 عرض المنتجات (للجميع)
        public async Task<IActionResult> Index(string category)
        {
            var products = await _productService.GetAllAsync(category);
            var categories = Enum.GetNames(typeof(ProductCategory)).ToList();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = category;

            return View(products);
        }

        // 🟢 إضافة منتج من التبرع (Admin فقط = قبول التبرع)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddFromDonation(int donationId)
        {
            var donation = await _donationService.GetDonationById(donationId);
            if (donation == null)
                return NotFound();

            var product = new Product
            {
                Name = donation.Name,
                ImgURL = donation.ImgURL,
                ExpirDate = donation.ExpirDate,
                Quantity = donation.Quantity,
                DonationId = donation.Id,
                Category = donation.Category
            };

            await _productService.AddAsync(product);

            // ✅ تحديث حالة التبرع → تم القبول
            donation.Status = DonationStatus.Accepted;
            await _donationService.UpdateDonation(donation);

            TempData["SuccessMessage"] = " تمت إضافة التبرع إلى قائمة المنتجات";
            return RedirectToAction("GetAll", "Donation");
        }

        // 🟢 حذف منتج (Admin فقط)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            TempData["SuccessMessage"] = " تم حذف المنتج بنجاح";
            return RedirectToAction("Products", "Admin");
        }
    }
}
