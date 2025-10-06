using FastFood.DAL.Data;
using FastFood.DAL.Models;
using Microsoft.AspNetCore.Identity;
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
        //Seed Roles 
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "User","Restaurant" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        //Gán Role Admin
        public static async Task SeedAdminAsync(UserManager<AppUserModel> userManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "Admin@123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new AppUserModel
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                     FullName = "FastFood Admin"
                    ,
                    CreatedDate = DateTime.Now
                };

                var result = await userManager.CreateAsync(adminUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
        // Seed Restaurant User
        public static async Task SeedRestaurantAsync(UserManager<AppUserModel> userManager)
        {
            string restaurantEmail = "restaurant@gmail.com";
            string password = "Restaurant@123";

            if (await userManager.FindByEmailAsync(restaurantEmail) == null)
            {
                var restaurantUser = new AppUserModel
                {
                    UserName = "fastfoodrestaurant",
                    Email = restaurantEmail,
                    EmailConfirmed = true,
                    FullName = "FastFood Restaurant"
                   ,
                    CreatedDate = DateTime.Now
                };

                var result = await userManager.CreateAsync(restaurantUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(restaurantUser, "Restaurant");
                }
            }
        }
    }

}