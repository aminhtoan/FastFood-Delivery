using Microsoft.EntityFrameworkCore;
using FastFood.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace FastFood.DAL.Data
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProductModel> Products { get; set; }
    }
}
