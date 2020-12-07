using Microsoft.EntityFrameworkCore;

namespace Shipping
{
    public class ShippingDbContext : DbContext
    {
        public DbSet<ShippingLabel> ShippingLabels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("LooselyCoupledMonolith_Shipping");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShippingLabel>()
                .HasKey(x => x.OrderId);

            base.OnModelCreating(modelBuilder);
        }
    }
}