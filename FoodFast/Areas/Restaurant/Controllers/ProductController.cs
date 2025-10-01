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
        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var product = await _productBLL.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(await _productBLL.GetAllCategoriesAsync(), "Id", "Name", product.CategoryId);
           
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                var result = await _productBLL.UpdateProductAsync(product);

                if (result.IsSuccess)
                {
                    TempData["success"] = result.Message;
                    return RedirectToAction("Index", "Product", new { area = "Restaurant" });
                }
                else
                {
                    TempData["error"] = result.Message;
                    ModelState.AddModelError("", result.Message);
                }
            }

            ViewBag.Categories = new SelectList(await _productBLL.GetAllCategoriesAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
       
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _productBLL.DeleteProductAsync(id);

            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
            }
            else
            {
                TempData["error"] = result.Message;
            }

            return RedirectToAction("Index", "Product", new { area = "Restaurant" });
        }
    }
}
