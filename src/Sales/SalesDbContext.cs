using Microsoft.EntityFrameworkCore;

namespace Sales
{
    public class SalesDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
        {

        }
    }
}