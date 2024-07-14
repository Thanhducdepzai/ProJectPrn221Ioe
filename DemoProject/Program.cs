
using Microsoft.EntityFrameworkCore;
using ProjectIoePrn.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<IOE_Project_Clone_PRN221Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("value")));
// Add services to the container.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<IOE_Project_Clone_PRN221Context>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // Giá trị HSTS mặc định là 30 ngày. Bạn có thể thay đổi giá trị này cho môi trường sản xuất
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();
app.UseSession();

app.Run();
