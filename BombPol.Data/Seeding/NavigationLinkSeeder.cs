using BombPol.Data.Entities;
using Microsoft.Extensions.Logging;

namespace BombPol.Data.Seeding
{
    /// <summary>
    /// Seeder for the NavigationLink table.
    /// </summary>
    public class NavigationLinkSeeder : ISeeder
    {
        private readonly BombPolContext _context;
        private readonly ILogger<NavigationLinkSeeder> _logger;

        public NavigationLinkSeeder(BombPolContext context, ILogger<NavigationLinkSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            var navLinks = GetNavigationLinks();
            await AddNavigationLinksToDatabaseAsync(navLinks);
        }

        /// <summary>
        /// Provides the list of navigation links to seed.
        /// </summary>
        private List<NavigationLink> GetNavigationLinks() 
            => new List<NavigationLink>
            {
                CreateHomeLink(),
                CreateProductsLink(),
                CreateCategoriesLink(),
                CreateOrdersLink(),
                CreateContactLink(),
                CreateAboutLink()
            };

        /// <summary>
        /// Creates the home navigation link.
        /// </summary>
        private NavigationLink CreateHomeLink() 
            => new NavigationLink
        {
            Id = Guid.NewGuid(),
            LinkTitle = "Home",
            ControllerName = "Home",
            ControllerAction = "Index",
            Url = "/"
        };

        /// <summary>
        /// Creates the products navigation link.
        /// </summary>
        private NavigationLink CreateProductsLink() 
            => new NavigationLink
        {
            Id = Guid.NewGuid(),
            LinkTitle = "Produkty",
            ControllerName = "Product",
            ControllerAction = "Index",
            Url = "/Product"
        };

        /// <summary>
        /// Creates the categories navigation link.
        /// </summary>
        private NavigationLink CreateCategoriesLink() 
            => new NavigationLink
        {
            Id = Guid.NewGuid(),
            LinkTitle = "Kategorie",
            ControllerName = "Category",
            ControllerAction = "Index",
            Url = "/Category"
        };

        /// <summary>
        /// Creates the orders navigation link.
        /// </summary>
        private NavigationLink CreateOrdersLink() 
            => new NavigationLink
        {
            Id = Guid.NewGuid(),
            LinkTitle = "Zamówienia",
            ControllerName = "Order",
            ControllerAction = "Index",
            Url = "/Order"
        };

        /// <summary>
        /// Creates the contact navigation link.
        /// </summary>
        private NavigationLink CreateContactLink() 
            => new NavigationLink
        {
            Id = Guid.NewGuid(),
            LinkTitle = "Kontakt",
            ControllerName = "Home",
            ControllerAction = "Contact",
            Url = "/Home/Contact"
        };

        /// <summary>
        /// Creates the about navigation link.
        /// </summary>
        private NavigationLink CreateAboutLink() 
            => new NavigationLink
        {
            Id = Guid.NewGuid(),
            LinkTitle = "O Nas",
            ControllerName = "Home",
            ControllerAction = "About",
            Url = "/Home/About"
        };

        /// <summary>
        /// Adds the navigation links to the database.
        /// </summary>
        private async Task AddNavigationLinksToDatabaseAsync(List<NavigationLink> navLinks)
        {
            _logger.LogInformation("Seeding navigation links...");
            await _context.NavigationLinks.AddRangeAsync(navLinks);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Seeded {Count} navigation links.", navLinks.Count);
        }
    }
}
