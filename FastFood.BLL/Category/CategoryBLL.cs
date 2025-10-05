using FastFood.DAL.Models;
using FastFood.DAL.Repository.Category;
using FastFood.DAL.Repository.Product;

namespace FastFood.BLL.Category
{
	public class CategoryBLL
	{
		private readonly CategoryDAL _categoryDAL;
        private readonly ProductDAL _productDAL;


        public CategoryBLL(CategoryDAL categoryDAL, ProductDAL productDAL)
		{
            _categoryDAL = categoryDAL;
            _productDAL = productDAL;

        }

		public IEnumerable<CategoryModel> GetAlLCategories()
		{
			// Có thể xử lý thêm nghiệp vụ ở đây (lọc, sắp xếp,…)
			return _categoryDAL.GetAll();
		}
        public async Task<List<ProductModel>> GetProductsByCategorySlugAsync(string slug)
        {
            var category = await _categoryDAL.GetCategoryBySlugAsync(slug);

            if (category == null)
            {
                return new List<ProductModel>(); // Không tìm thấy category
            }

            return await _productDAL.GetProductsByCategoryIdAsync(category.Id);
        }
    }
}
