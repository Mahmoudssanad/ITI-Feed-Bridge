using Feed_Bridge.IServices;
using Feed_Bridge.Models.Entities;
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
        public async Task<IActionResult> Index()
        {
            var allProducts = await _productService.GetAllAsync();
            return View(allProducts);
        }
        //ttpGet]
        //public async Task<IActionResult> Edit( int Id)
        //{
        //    var product = await _productService.GetByIdAsync( Id);
        //    if (product == null) return NotFound();

        //    var viewModel = new EditProductViewModel
        //    {
        //        Id = product.Id,
        //        Name = product.Name,
        //        ExpirDate = product.ExpirDate,
        //        Quantity = product.Quantity,
        //        Address = product.Donation?.Address,
        //        Phone = product.Donation?.Phone,
        //        Description = product.Donation?.Description,

        //        ExistingImageUrl = product.ImgURL
        //    };

        //    return View(viewModel);
        //}
      


    }

}
