using Microsoft.EntityFrameworkCore;

namespace Sales
{
    public class SalesDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("LooselyCoupledMonolith_Sales");
            base.OnConfiguring(optionsBuilder);
        }
    }
}