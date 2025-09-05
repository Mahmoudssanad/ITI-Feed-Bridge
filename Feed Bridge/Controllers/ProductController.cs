using Feed_Bridge.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Feed_Bridge.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var allProducts = await _productService.GetAllAsync();
            return View(allProducts);
        }
    }
}
