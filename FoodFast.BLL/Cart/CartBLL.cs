using FoodFast.DAL.Models;
using FoodFast.DAL.Models.ViewModels;
using FoodFast.DAL.Repository.Cart;

namespace FoodFast.BLL.Cart
{
    public class CartBLL
    {
        private readonly CartDAL _cartDAL;

        public CartBLL(CartDAL cartDAL)
        {
            _cartDAL = cartDAL;
        }

        public CartItemViewModel BuildCartViewModel(List<CartItemModel> cartItems)
        {
            return new CartItemViewModel
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
            };
        }
        public async Task<List<CartItemModel>> AddToCartAsync(List<CartItemModel> cart, long productId)
        {
            var product = await _cartDAL.GetProductByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");
            // 2. Tìm xem sản phẩm này đã có trong giỏ hàng chưa
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem == null)
                cart.Add(new CartItemModel(product));
            else
                cartItem.Quantity += 1;

            return cart;
        }
        public List<CartItemModel> DecreaseProduct(List<CartItemModel> cart, long productId)
        {
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity -= 1;
                }
                else
                {
                    cart.RemoveAll(p => p.ProductId == productId);
                }
            }

            return cart;
        }
        public List<CartItemModel> IncreaseProduct(List<CartItemModel> cart, long productId)
        {
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                if (cartItem.Quantity >= 1)
                {
                    cartItem.Quantity += 1;
                }
                else
                {
                    cart.RemoveAll(p => p.ProductId == productId);
                }
            }

            return cart;
        }
    }
}
