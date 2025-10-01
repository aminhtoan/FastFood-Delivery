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
        // Lấy sản phẩm theo ID
        public async Task<ProductModel?> GetProductByIdAsync(long id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ProductModel>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                                 .Where(p => p.CategoryId == categoryId)
                                 .OrderByDescending(p => p.Id)
                                 .ToListAsync();
        }
        // Lấy tất cả danh mục
        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        public async Task<ProductModel?> GetProductBySlugAsync(string slug)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task AddProductAsync(ProductModel product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        // Cập nhật sản phẩm
        public async Task UpdateProductAsync(ProductModel product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Xóa sản phẩm
        public async Task DeleteProductAsync(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
