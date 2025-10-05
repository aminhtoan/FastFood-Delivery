
using Microsoft.AspNetCore.Mvc;

using FastFood.BLL.Cart;
using FastFood.DAL.Data;
using FastFood.DAL.Models;

using FastFood.UI.ViewModels;

namespace FastFood.Controllers
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
            // 1. Lấy cart từ Session (UI responsibility)
            var cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart")
                            ?? new List<CartItemModel>();

            // 2. Gọi BLL để xử lý business (tính tổng tiền)
            decimal grandTotal = _cartBLL.CalculateGrandTotal(cartItems);

            // 3. Map sang ViewModel
            var cartVM = new CartItemViewModel
            {
                CartItems = cartItems,
                GrandTotal = grandTotal
            };

            // 4. Trả về view
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
    

