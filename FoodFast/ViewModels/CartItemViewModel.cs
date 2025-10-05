using FastFood.DAL.Models;

namespace FastFood.UI.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> CartItems { get; set; } = new();
        public decimal GrandTotal { get; set; }
    }
}
