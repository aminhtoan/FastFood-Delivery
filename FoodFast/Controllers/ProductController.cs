using FastFood.BLL.Product;
using FastFood.DAL.Models;
using FastFood.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductBLL _productBLL;
        public ProductController(ProductBLL productBLL)
        {
            _productBLL = productBLL;

        }
        public async Task<IActionResult> Index()
        {
            var products = await _productBLL.GetAllProductsAsync(); // Trả về List<ProductDTO>

            // Map DTO sang ProductViewModel
            var viewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName ?? string.Empty,
                Image = p.Image
            }).ToList();

            return View(viewModels);
        }
    }
}
