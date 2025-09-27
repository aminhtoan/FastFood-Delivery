using FoodFast.DAL.Data;
using FoodFast.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodFast.DAL.Repository.Product
{
    public class ProductDAL
    {
        private readonly DataContext _context;

        public ProductDAL(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductModel> GetAllProducts()
        {
            return _context.Products.Include(p => p.Category).ToList();
        }
        public async Task<List<ProductModel>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                                 .Where(p => p.CategoryId == categoryId)
                                 .OrderByDescending(p => p.Id)
                                 .ToListAsync();
        }
    }
}
