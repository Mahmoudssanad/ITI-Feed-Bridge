using Feed_Bridge.IServices;
using Feed_Bridge.Models.Entities;
using Feed_Bridge.Models.Enums;
using Feed_Bridge.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feed_Bridge.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index(string category)
        {
            var products = await _productService.GetAllAsync(category);

            // جلب جميع أسماء الكاتيجوري من الـ enum كـ List<string>
            var categories = Enum.GetNames(typeof(ProductCategory)).ToList();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = category;

            return View(products);
        }



        //public async Task<IActionResult> Index(string category)
        //{
        //    // جلب المنتجات بالفلاتر (تاريخ + كاتيجوري)
        //    var products = await _productService.GetAllAsync(category,search );

        //    // جلب قائمة كل الكاتيجوريز من الـ Enum
        //    var categories = Enum.GetNames(typeof(ProductCategory)).ToList();

        //    // تمرير القيم للفيو
        //    ViewBag.Categories = categories;
        //    ViewBag.SelectedCategory = category;

        //    return View(products);
        //}
        //public async Task<IActionResult> Index(string category)
        //{
        //    var products = await _productService.GetAllAsync(category);
        //    ViewBag.SelectedCategory = category;
        //    return View(products);
        //}

        //public async Task<IActionResult> Index(ProductCategory? category)
        //{
        //    var products = await _productService.GetAllAsync();

        //    if (category.HasValue)
        //    {
        //        products = products.Where(p => p.Category == category.Value);
        //    }

        //    return View(products);
        //}
        // GET: Delete
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")] // هنا بقول للـ routing اعتبرها Delete برضه
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                TempData["SuccessMessage"] = "تم حذف المنتج بنجاح";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حصل خطأ أثناء الحذف: " + ex.Message;
            }

            return RedirectToAction("Products", "Admin");
        }



    }

}
