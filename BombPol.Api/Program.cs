using BombPol.Data;
using Microsoft.EntityFrameworkCore;

namespace BombPol.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            BuildServices(builder);
            InitializeAppAndRun(builder);
        }

        private static void InitializeAppAndRun(WebApplicationBuilder builder)
        {
            var app = builder.Build();
            app.Services.SeedDatabaseAsync().Wait();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static void BuildServices(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<BombPolContext>(options =>
                            options.UseSqlServer(builder.Configuration.GetConnectionString("BombPolContext")
                            ?? throw new InvalidOperationException("Connection string 'BombPolContext' not found.")));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}