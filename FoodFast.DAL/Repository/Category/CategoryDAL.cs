using FoodFast.DAL.Data;
using FoodFast.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodFast.DAL.Repository.Category
{
	public class CategoryDAL
	{
		private readonly DataContext _context;

		public CategoryDAL(DataContext context)
		{
			_context = context;
		}

		public IEnumerable<CategoryModel> GetAll()
		{
			return _context.Categories.ToList();
		}
        public async Task<CategoryModel?> GetCategoryBySlugAsync(string slug)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
        }
    }
}
