using BombPol.Data.Entities;
using Microsoft.Extensions.Logging;

namespace BombPol.Data.Seeding
{
    /// <summary>
    /// Seeder for the Category table.
    /// </summary>
    public class CategorySeeder : ISeeder
    {
        private readonly ILogger<CategorySeeder> _logger;
        private readonly BombPolContext _context;

        public CategorySeeder(BombPolContext context, ILogger<CategorySeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            var categories = GetCategories();
            await AddCategoriesToDatabaseAsync(categories);
        }

        /// <summary>
        /// Provides the list of categories to seed.
        /// </summary>
        private List<Category> GetCategories()
            => new List<Category>
            {
                CreateNoiseCategory(),
                CreateLightCategory(),
                CreateSetCategory()
            };

        /// <summary>
        /// Creates a noise/sound category.
        /// </summary>
        private Category CreateNoiseCategory()
            => new Category(
                Guid.NewGuid(),
                "Ha³asowe",
                "Petardy i bomby ha³asowe — du¿y efekt dŸwiêkowy, odpowiednie oznaczenia i instrukcje bezpieczeñstwa."
            );

        /// <summary>
        /// Creates a light/visual category.
        /// </summary>
        private Category CreateLightCategory()
            => new Category(
                Guid.NewGuid(),
                "Œwietlne",
                "Bengalskie, fontanny i sztuczne ognie — estetyczne efekty œwietlne."
            );

        /// <summary>
        /// Creates a sets/bundles category.
        /// </summary>
        private Category CreateSetCategory() 
            => new Category(
                Guid.NewGuid(),
                "Zestawy",
                "Gotowe zestawy mieszane — zestawy na imprezy i pokazy."
            );

        /// <summary>
        /// Adds the categories to the database.
        /// </summary>
        private async Task AddCategoriesToDatabaseAsync(List<Category> categories)
        {
            _logger.LogInformation("Seeding categories...");
            await _context.Category.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Seeded {Count} categories.", categories.Count);
        }
    }
}