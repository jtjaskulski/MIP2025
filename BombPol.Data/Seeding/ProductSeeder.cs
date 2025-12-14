using BombPol.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BombPol.Data.Seeding
{
    /// <summary>
    /// Seeder for the Product table.
    /// </summary>
    public class ProductSeeder : ISeeder
    {
        private readonly BombPolContext _context;
        private readonly ILogger<ProductSeeder> _logger;

        public ProductSeeder(BombPolContext context, ILogger<ProductSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            var categories = await GetCategoriesFromDatabaseAsync();
            
            if (!ValidateCategories(categories))
            {
                return;
            }

            var products = GetProducts(categories);
            await AddProductsToDatabaseAsync(products);
        }

        /// <summary>
        /// Retrieves categories from the database.
        /// </summary>
        private async Task<Dictionary<string, Category>> GetCategoriesFromDatabaseAsync() 
            => (await _context.Category.AsNoTracking().ToListAsync()).ToDictionary(c => c.Name, c => c);

        /// <summary>
        /// Validates that all required categories exist.
        /// </summary>
        private bool ValidateCategories(Dictionary<string, Category> categories)
        {
            if (!categories.ContainsKey("Ha³asowe") || 
                !categories.ContainsKey("Œwietlne") || 
                !categories.ContainsKey("Zestawy"))
            {
                _logger.LogWarning("Not all categories found. Skipping product seeding.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Provides the list of products to seed.
        /// </summary>
        private List<Product> GetProducts(Dictionary<string, Category> categories)
        {
            var products = new List<Product>();
            
            products.AddRange(GetNoiseProducts(categories["Ha³asowe"].Id));
            products.AddRange(GetLightProducts(categories["Œwietlne"].Id));
            products.AddRange(GetSetProducts(categories["Zestawy"].Id));
            
            return products;
        }

        /// <summary>
        /// Gets noise/sound products.
        /// </summary>
        private List<Product> GetNoiseProducts(Guid categoryId) 
            => new List<Product>
            {
                CreateProduct("Bomba Ha³asowa Pro", "Profesjonalna bomba ha³asowa dla imprez i wydarzeñ — d³ugi huk.", 49.99m, categoryId),
                CreateProduct("Petarda Sygna³owa", "Ma³a petarda sygna³owa — krótki, g³oœny efekt.", 19.99m, categoryId),
                CreateProduct("Mega Boom XL", "Bomba o zwiêkszonym efekcie dla profesjonalnych pokazów.", 89.99m, categoryId),
                CreateProduct("Thunder Blast", "Potê¿na petarda z d³ugotrwa³ym efektem ha³asu.", 34.50m, categoryId),
                CreateProduct("Boom Compact", "Kompaktowa bomba ha³asowa, idealna do przewozu.", 12.99m, categoryId)
            };

        /// <summary>
        /// Gets light/visual products.
        /// </summary>
        private List<Product> GetLightProducts(Guid categoryId) 
            => new List<Product>
            {
                CreateProduct("Bengalski Z³oty 60s", "Z³oty bengalski z d³ugotrwa³ym efektem œwietlnym.", 29.99m, categoryId),
                CreateProduct("Fontanna Srebrna", "Estetyczna fontanna na sceny i dekoracje.", 39.99m, categoryId),
                CreateProduct("Laser Spark", "Niskie natê¿enie œwiat³a, dekoracyjny efekt.", 24.50m, categoryId),
                CreateProduct("Bengal Srebrny 120s", "Przed³u¿ony bengalski ze srebrzystym efektem — idealny do fotografowania.", 44.99m, categoryId),
                CreateProduct("Fontanna Têczowa", "Kolorowa fontanna z wieloma efektami œwietlnymi.", 54.99m, categoryId),
                CreateProduct("Luz Kaskadowy", "Efekt kaskady œwiat³a — estetyczny dla bo¿onarodzeniowych dekoracji.", 19.99m, categoryId)
            };

        /// <summary>
        /// Gets set/bundle products.
        /// </summary>
        private List<Product> GetSetProducts(Guid categoryId) 
            => new List<Product>
            {
                CreateProduct("Zestaw Mieszany Deluxe", "Kompletny zestaw: 2 bomby ha³asowe, 3 bengalskie, 1 fontanna.", 149.99m, categoryId),
                CreateProduct("Zestaw Startowy", "Zestaw dla pocz¹tkuj¹cych — bezpieczne próbki efektów œwietlnych i dŸwiêkowych.", 59.99m, categoryId),
                CreateProduct("Zestaw Premium Party", "Zaawansowany zestaw do imprez — 4 bomby, 5 bengalskich, 2 fontanny.", 249.99m, categoryId),
                CreateProduct("Zestaw Noworoczny", "Specjalny zestaw na Nowy Rok z ró¿nymi efektami.", 199.99m, categoryId)
            };

        /// <summary>
        /// Creates a product with the specified properties.
        /// </summary>
        private Product CreateProduct(string name, string description, decimal price, Guid categoryId) 
            => new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            CategoryId = categoryId
        };

        /// <summary>
        /// Adds the products to the database.
        /// </summary>
        private async Task AddProductsToDatabaseAsync(List<Product> products)
        {
            _logger.LogInformation("Seeding products...");
            await _context.Product.AddRangeAsync(products);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Seeded {Count} products.", products.Count);
        }
    }
}
