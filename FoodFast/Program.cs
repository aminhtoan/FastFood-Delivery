using Microsoft.EntityFrameworkCore;
using FoodFast.DAL.Data;
using FoodFast.BLL.Product;
using FoodFast.DAL.Repository.Product;



var builder = WebApplication.CreateBuilder(args);

//Connect Db
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]);
});
//Add service
builder.Services.AddControllersWithViews();
// Add Service BLL
builder.Services.AddScoped<ProductDAL>();
builder.Services.AddScoped<ProductBLL>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//SeedingData
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedingData(context);

app.Run();
