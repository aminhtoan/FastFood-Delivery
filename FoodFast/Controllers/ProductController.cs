using FoodFast.BLL.Product;
using FoodFast.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodFast.Controllers
{
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
    }
}
