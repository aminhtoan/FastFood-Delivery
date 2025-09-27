
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
        public IActionResult Increase(long Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart")
                                        ?? new List<CartItemModel>();

            cart = _cartBLL.IncreaseProduct(cart, Id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["success"] = "Increase Product from cart successfully!";
            return RedirectToAction("Index");
        }
        public IActionResult Remove(long Id)
        {
            var cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            if (cart == null) return RedirectToAction("Index");  // Tránh lỗi nếu giỏ hàng rỗng


            cart.RemoveAll(p => p.ProductId == Id);  // Xóa sản phẩm khỏi giỏ hàng

            if (cart.Count > 0)
            {
                HttpContext.Session.SetJson("Cart", cart);  // Cập nhật lại giỏ hàng nếu còn sản phẩm
            }
            else
            {
                HttpContext.Session.Remove("Cart");  // Xóa giỏ hàng khỏi Session nếu trống
            }

            return RedirectToAction("Index");
        }
    }
    }
    

