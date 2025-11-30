using Microsoft.EntityFrameworkCore;
using BombPol.Data.Entities;

namespace BombPol.Data
{
    public class BombPolContext : DbContext
    {
        public BombPolContext (DbContextOptions<BombPolContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<OrderItem> OrderItem { get; set; } = default!;

        public override int SaveChanges()
        {
            HandleSoftDelete();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleSoftDelete();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void HandleSoftDelete()
        {
            var entries = ChangeTracker.Entries<SoftDeleteBusinessModel>();

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.ModifiedAt = now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = now;
                        entry.Entity.DeletedAt = null;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedAt = now;
                        entry.Entity.ModifiedAt = now;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
        }
    }
}