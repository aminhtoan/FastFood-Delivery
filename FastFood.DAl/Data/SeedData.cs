using FastFood.DAL.Data;
using FastFood.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DAL.Data
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())// điều kiện là nếu Product rỗng
            {
                CategoryModel pho = new CategoryModel { Name = "Pho", Slug = "pho", Description = "Món phở truyền thống Việt Nam với nước dùng đậm đà", Status = 1 };
                CategoryModel com = new CategoryModel { Name = "Com", Slug = "com", Description = "Cơm tấm, cơm chiên và các món cơm khác", Status = 1 };
                _context.Products.AddRange(
                    new ProductModel { Name = "Phở gà", Slug = "phoga", Description = "Phở gà thanh đạm, nước dùng ngọt tự nhiên", Image = "1.jpg", Category = pho,  Price = 40000 },
                    new ProductModel { Name = "Cơm tấm", Slug = "comtam", Description = "Cơm tấm với sườn nướng, chả trứng, bì", Image = "1.jpg", Category = com,  Price = 35000 }
                );

                _context.SaveChanges();
            }
           
        }
    }

}