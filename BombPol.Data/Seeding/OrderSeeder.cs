using BombPol.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BombPol.Data.Seeding
{
    /// <summary>
    /// Seeder for the Order and OrderItem tables.
    /// </summary>
    public class OrderSeeder : ISeeder
    {
        private readonly BombPolContext _context;
        private readonly ILogger<OrderSeeder> _logger;

        public OrderSeeder(BombPolContext context, ILogger<OrderSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            var products = await GetProductsFromDatabaseAsync();
            
            if (!ValidateProducts(products))
            {
                return;
            }

            var ordersWithItems = GetOrdersWithItems(products);
            await AddOrdersToDatabaseAsync(ordersWithItems);
        }

        /// <summary>
        /// Retrieves products from the database.
        /// </summary>
        private async Task<List<Product>> GetProductsFromDatabaseAsync() 
            => await _context.Product.AsNoTracking().ToListAsync();

        /// <summary>
        /// Validates that products exist in the database.
        /// </summary>
        private bool ValidateProducts(List<Product> products)
        {
            if (!products.Any())
            {
                _logger.LogWarning("No products found. Skipping order seeding.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Provides the list of orders with their items.
        /// </summary>
        private List<Order> GetOrdersWithItems(List<Product> products) 
            => new List<Order>
            {
                CreateJanKowalskiOrder(products),
                CreateMariaNowakOrder(products),
                CreatePiotrLewandowskiOrder(products),
                CreateAnnaSzymanskaOrder(products)
            };

        /// <summary>
        /// Creates Jan Kowalski's order.
        /// </summary>
        private Order CreateJanKowalskiOrder(List<Product> products)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = "Jan Kowalski",
                CustomerEmail = "jan.kowalski@example.com",
                CustomerPhone = "+48 600 000 000",
                Status = OrderStatus.Pending,
                TotalAmount = 0m
            };

            var items = new List<OrderItem>
            {
                CreateOrderItem(order.Id, products[0], 2),
                CreateOrderItem(order.Id, products[5], 1)
            };

            order.TotalAmount = items.Sum(i => i.TotalPrice);
            order.OrderItems = items;
            return order;
        }

        /// <summary>
        /// Creates Maria Nowak's order.
        /// </summary>
        private Order CreateMariaNowakOrder(List<Product> products)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = "Maria Nowak",
                CustomerEmail = "maria.nowak@example.com",
                CustomerPhone = "+48 601 111 111",
                Status = OrderStatus.Confirmed,
                TotalAmount = 0m
            };

            var items = new List<OrderItem>
            {
                CreateOrderItem(order.Id, products[11], 1),
                CreateOrderItem(order.Id, products[6], 2),
                CreateOrderItem(order.Id, products[1], 3)
            };

            order.TotalAmount = items.Sum(i => i.TotalPrice);
            order.OrderItems = items;
            return order;
        }

        /// <summary>
        /// Creates Piotr Lewandowski's order.
        /// </summary>
        private Order CreatePiotrLewandowskiOrder(List<Product> products)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = "Piotr Lewandowski",
                CustomerEmail = "piotr.lewandowski@example.com",
                CustomerPhone = "+48 602 222 222",
                Status = OrderStatus.Shipped,
                TotalAmount = 0m
            };

            var items = new List<OrderItem>
            {
                CreateOrderItem(order.Id, products[13], 1),
                CreateOrderItem(order.Id, products[9], 1)
            };

            order.TotalAmount = items.Sum(i => i.TotalPrice);
            order.OrderItems = items;
            return order;
        }

        /// <summary>
        /// Creates Anna Szymañska's order.
        /// </summary>
        private Order CreateAnnaSzymanskaOrder(List<Product> products)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = "Anna Szymañska",
                CustomerEmail = "anna.szymanska@example.com",
                CustomerPhone = "+48 603 333 333",
                Status = OrderStatus.Pending,
                TotalAmount = 0m
            };

            var items = new List<OrderItem>
            {
                CreateOrderItem(order.Id, products[12], 2),
                CreateOrderItem(order.Id, products[3], 4)
            };

            order.TotalAmount = items.Sum(i => i.TotalPrice);
            order.OrderItems = items;
            return order;
        }

        /// <summary>
        /// Creates an order item with calculated totals.
        /// </summary>
        private OrderItem CreateOrderItem(Guid orderId, Product product, int quantity) 
            => new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ProductId = product.Id,
            Quantity = quantity,
            UnitPrice = product.Price,
            TotalPrice = product.Price * quantity
        };

        /// <summary>
        /// Adds the orders and order items to the database.
        /// </summary>
        private async Task AddOrdersToDatabaseAsync(List<Order> orders)
        {
            _logger.LogInformation("Seeding orders and order items...");
            
            var orderItems = new List<OrderItem>();
            foreach (var order in orders)
            {
                if (order.OrderItems != null)
                {
                    orderItems.AddRange(order.OrderItems);
                }
            }

            await _context.Order.AddRangeAsync(orders);
            await _context.OrderItem.AddRangeAsync(orderItems);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Seeded {Count} orders with {ItemCount} order items.", orders.Count, orderItems.Count);
        }
    }
}
