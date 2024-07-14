
using Microsoft.EntityFrameworkCore;
using ProjectIoePrn.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<IOE_Project_Clone_PRN221Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("value")));
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<IOE_Project_Clone_PRN221Context>();
builder.Services.AddSession();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseSession();
app.Run();
