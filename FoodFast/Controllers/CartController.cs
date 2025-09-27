
using Microsoft.AspNetCore.Mvc;

using FoodFast.BLL.Cart;
using FoodFast.DAL.Data;
using FoodFast.DAL.Models;

namespace FoodFast.Controllers
{
    public class CartController : Controller
    {
       
            private readonly CartBLL _cartBLL;

        public CartController(CartBLL cartBLL)
        {
            _cartBLL = cartBLL;
        }

        public IActionResult Index()
        {
            // Lấy cart từ session
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart")
                                           ?? new List<CartItemModel>();

            // Gọi BLL xử lý logic
            var cartVM = _cartBLL.BuildCartViewModel(cartItems);

            // Trả về View
            return View(cartVM);
        }
       
        public async Task<IActionResult> Add(long Id)
        {
            // Lấy giỏ hàng từ Session
            var cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            // Gọi BLL để xử lý
            cart = await _cartBLL.AddToCartAsync(cart, Id);

            // Lưu lại giỏ hàng vào Session
            HttpContext.Session.SetJson("Cart", cart);

            TempData["success"] = "Add Item to Cart successfully";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult Decrease(long Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart")
                                        ?? new List<CartItemModel>();

            cart = _cartBLL.DecreaseProduct(cart, Id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["success"] = "Decrease Product from cart successfully!";
            return RedirectToAction("Index");
        }
    }
    }
    

