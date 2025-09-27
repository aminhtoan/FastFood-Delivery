using FoodFast.DAL.Data;
using FoodFast.DAL.Models;

namespace FoodFast.DAL.Repository.Cart
{
    public class CartDAL
    {
        private readonly DataContext _dataContext;

        public CartDAL(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ProductModel> GetProductByIdAsync(long id)
        {
            return await _dataContext.Products.FindAsync(id);
        }

    }
}
