using BombPol.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BombPol.Data
{
    /// <summary>
    /// Orchestrates database seeding across all tables.
    /// </summary>
    public class BombPolContextDataSeed
    {
        private readonly ILogger<BombPolContext> _logger;
        private readonly BombPolContext _context;
        private readonly CategorySeeder _categorySeeder;
        private readonly ProductSeeder _productSeeder;
        private readonly NavigationLinkSeeder _navigationLinkSeeder;
        private readonly OrderSeeder _orderSeeder;

        public BombPolContextDataSeed(BombPolContext context, ILogger<BombPolContext> logger, CategorySeeder categorySeeder, ProductSeeder productSeeder, 
            NavigationLinkSeeder navigationLinkSeeder, OrderSeeder orderSeeder)
        {
            _context = context;
            _logger = logger;
            _categorySeeder = categorySeeder;
            _productSeeder = productSeeder;
            _navigationLinkSeeder = navigationLinkSeeder;
            _orderSeeder = orderSeeder;
        }

        /// <summary>
        /// Seeds the database with initial data if it is empty.
        /// </summary>
        public async Task SeedDatabaseIfEmptyAsync()
        {
            try
            {
                await ApplyMigrationsAsync();
                
                if (await IsDatabaseAlreadySeededAsync())
                {
                    return;
                }

                _logger.LogInformation("Database appears empty — starting seed.");
                await SeedAllTablesAsync();
                _logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        /// <summary>
        /// Applies pending migrations to the database.
        /// </summary>
        private async Task ApplyMigrationsAsync()
        {
            _logger.LogInformation("Applying migrations (if any)...");
            await _context.Database.MigrateAsync();
        }

        /// <summary>
        /// Checks if the database has already been seeded.
        /// </summary>
        private async Task<bool> IsDatabaseAlreadySeededAsync()
        {
            var hasCategories = await _context.Category.AsNoTracking().AnyAsync();
            var hasProducts = await _context.Product.AsNoTracking().AnyAsync();
            var hasNavLinks = await _context.NavigationLinks.AsNoTracking().AnyAsync();

            if (hasCategories || hasProducts || hasNavLinks)
            {
                _logger.LogInformation("Database already contains data. Skipping seed.");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Seeds all tables in the database.
        /// </summary>
        private async Task SeedAllTablesAsync()
        {
            await SeedCategoriesTableAsync();
            await SeedProductsTableAsync();
            await SeedNavigationLinksTableAsync();
            await SeedOrdersTableAsync();
        }

        /// <summary>
        /// Seeds the Category table.
        /// </summary>
        private async Task SeedCategoriesTableAsync() => await _categorySeeder.SeedAsync();

        /// <summary>
        /// Seeds the Product table.
        /// </summary>
        private async Task SeedProductsTableAsync() => await _productSeeder.SeedAsync();

        /// <summary>
        /// Seeds the NavigationLink table.
        /// </summary>
        private async Task SeedNavigationLinksTableAsync() => await _navigationLinkSeeder.SeedAsync();

        /// <summary>
        /// Seeds the Order and OrderItem tables.
        /// </summary>
        private async Task SeedOrdersTableAsync() => await _orderSeeder.SeedAsync();
    }
}