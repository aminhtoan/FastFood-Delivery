

using FastFood.BLL.Category;
using FastFood.DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Repository.Components
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
