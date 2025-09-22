

using FoodFast.BLL.Category;
using FoodFast.DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodFast.Repository.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
		private readonly CategoryBLL _categoryBLL;
		public CategoriesViewComponent(CategoryBLL categoryBLL)
        {
            _categoryBLL = categoryBLL;
        }
		public IViewComponentResult Invoke()
		{
			var categories = _categoryBLL.GetAlLCategories();
			return View(categories);
		}
	}
}
