

using FoodFast.BLL.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodFast.DAL.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
		private readonly CategoryBLL _categoryBLL;
		public CategoriesViewComponent(CategoryBLL categoryBLL)
        {
            _categoryBLL = categoryBLL;
        }
        public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.Categories.ToListAsync());
    }
}
