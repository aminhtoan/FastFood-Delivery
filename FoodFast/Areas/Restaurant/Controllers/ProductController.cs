using FoodFast.BLL.Product;
using FoodFast.DAL.Data;
using FoodFast.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FastFood.UI.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    public class ProductController : Controller
    {
        private readonly ProductBLL _productBLL;
      
      
        public ProductController(ProductBLL productBLL)
        {
            _productBLL = productBLL;

        }
        public IActionResult Index()
        {
            var products = _productBLL.GetAllProducts();

            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _productBLL.GetAllCategoriesAsync(), "Id", "Name");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                var result = await _productBLL.CreateProductAsync(product);

                if (result.IsSuccess)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Index", "Product", new { area = "Restaurant" });
                }
                else
                {
                    TempData["Error"] = result.Message;
                    ModelState.AddModelError("", result.Message);
                }
            }
            
            else
            {
                // Thu thập tất cả lỗi validation
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["Error"] = "Vui lòng kiểm tra lại dữ liệu: " + string.Join(", ", errors);
            }

            // Load lại dropdown lists nếu có lỗi
            ViewBag.Categories = new SelectList(await _productBLL.GetAllCategoriesAsync(), "Id", "Name", product.CategoryId);
          
            return View(product);
        }
    }
}
