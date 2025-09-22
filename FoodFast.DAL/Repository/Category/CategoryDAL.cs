using FoodFast.DAL.Data;
using FoodFast.DAL.Models;
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
	}
}
