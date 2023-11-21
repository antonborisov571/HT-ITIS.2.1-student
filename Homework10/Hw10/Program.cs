// dotcover disable
using System.Diagnostics.CodeAnalysis;
using Hw10.Configuration;
using Hw10.DbModels;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddMathCalculator()
    .AddCachedMathCalculator();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=userdb;Username=postgres;Password=1234"));

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calculator}/{action=Index}/{id?}");
app.Run();

namespace Hw10
{
    [ExcludeFromCodeCoverage]
    public partial class Program { }
}