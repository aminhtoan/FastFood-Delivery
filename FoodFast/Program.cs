using Microsoft.EntityFrameworkCore;
using FastFood.DAL.Data;
using FastFood.BLL.Product;
using FastFood.DAL.Repository.Product;
using FastFood.BLL.Category;
using FastFood.DAL.Repository.Category;
using FastFood.BLL.Cart;
using FastFood.DAL.Repository.Cart;



var builder = WebApplication.CreateBuilder(args);

//Connect Db
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]);
});
//Add service
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});
// Add Service Product
builder.Services.AddScoped<ProductDAL>();
builder.Services.AddScoped<ProductBLL>();
// Add Service Category
builder.Services.AddScoped<CategoryDAL>();
builder.Services.AddScoped<CategoryBLL>();
// Add Service Cart
builder.Services.AddScoped<CartDAL>();
builder.Services.AddScoped<CartBLL>();
var app = builder.Build();
app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
app.UseSession();
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
//Area Route
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "category",
    pattern: "/category/{Slug?}",
    defaults: new { controller = "Category", action = "Index" });

//Custom Route lưu ý phải đặt trên default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



//SeedingData
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedingData(context);

app.Run();
