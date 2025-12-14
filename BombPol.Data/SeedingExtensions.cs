using BombPol.Data.Seeding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BombPol.Data
{
    /// <summary>
    /// Extension methods for database seeding during application startup.
    /// </summary>
    public static class SeedingExtensions
    {
        /// <summary>
        /// Seeds the database with initial data if empty. Call this after building the WebApplication
        /// and before app.Run().
        /// </summary>
        /// <param name="services">The IServiceProvider instance.</param>
        /// <returns>An async task that completes when seeding is done.</returns>
        /// <example>
        /// var app = builder.Build();
        /// await app.Services.SeedDatabaseAsync();
        /// app.Run();
        /// </example>
        public static async Task SeedDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<BombPolContext>();
            var logger = scopedServices.GetRequiredService<ILogger<BombPolContext>>();
            var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();

            var categorySeeder = new CategorySeeder(context, loggerFactory.CreateLogger<CategorySeeder>());
            var productSeeder = new ProductSeeder(context, loggerFactory.CreateLogger<ProductSeeder>());
            var navigationLinkSeeder = new NavigationLinkSeeder(context, loggerFactory.CreateLogger<NavigationLinkSeeder>());
            var orderSeeder = new OrderSeeder(context, loggerFactory.CreateLogger<OrderSeeder>());

            var seeder = new BombPolContextDataSeed(context, logger, categorySeeder, productSeeder, navigationLinkSeeder, orderSeeder);
            await seeder.SeedDatabaseIfEmptyAsync();
        }
    }
}