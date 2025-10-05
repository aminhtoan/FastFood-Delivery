using FastFood.DAL.Data;
using FastFood.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FastFood.DAL.Repository.Category
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
