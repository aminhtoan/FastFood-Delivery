using FastFood.BLL.Category;
using FastFood.DAL.Repository.Product;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryBLL _categoryBLL;

        public CategoryController(CategoryBLL categoryBLL)
        {
            _categoryBLL = categoryBLL;
        }

        public async Task<IActionResult> Index(string Slug = "")
        {
            var products = await _categoryBLL.GetProductsByCategorySlugAsync(Slug);

            if (products.Count == 0)
            {
                return RedirectToAction("Index", "Product");
            }

            return View(products);
        }
    }
}
