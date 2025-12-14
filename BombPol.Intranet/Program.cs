using Microsoft.EntityFrameworkCore;
using BombPol.Data;

namespace BombPol.Intranet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<BombPolContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BombPolContext") ?? throw new InvalidOperationException("Connection string 'BombPolContext' not found.")));

            builder.Services.AddControllersWithViews();

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

            app.Services.SeedDatabaseAsync().Wait();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}