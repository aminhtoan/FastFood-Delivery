using FoodFast.DAL.Models;
using FoodFast.DAL.Repository.Category;
using FoodFast.DAL.Repository.Product;

namespace FoodFast.BLL.Category
{
	public class CategoryBLL
	{
		private readonly CategoryDAL _categoryDAL;

		public CategoryBLL(CategoryDAL categoryDAL)
		{
			_categoryDAL = categoryDAL;
		}

		public IEnumerable<CategoryModel> GetAlLCategories()
		{
			// Có thể xử lý thêm nghiệp vụ ở đây (lọc, sắp xếp,…)
			return _categoryDAL.GetAll();
		}
	}
}
