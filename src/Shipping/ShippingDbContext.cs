using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shipping
{
    public class ShippingDbContext : DbContext
    {
        public DbSet<ShippingLabel> ShippingLabels { get; set; }
        public DbSet<IdempotentConsumer> IdempotentConsumers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLExpress;Database=Demo;Trusted_Connection=Yes;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShippingLabel>()
                .ToTable("ShippingLabels")
                .HasKey(x => x.OrderId);

            modelBuilder.Entity<IdempotentConsumer>()
                .ToTable("IdempotentConsumers")
                .HasKey(x => new {x.MessageId, x.Consumer});

            base.OnModelCreating(modelBuilder);
        }

        public async Task IdempotentConsumer(long messageId, string consumer)
        {
            await IdempotentConsumers.AddAsync(new IdempotentConsumer
            {
                MessageId = messageId,
                Consumer = consumer
            });
            await SaveChangesAsync();
        }

        public async Task<bool> HasBeenProcessed(long messageId, string consumer)
        {
            return await IdempotentConsumers.AnyAsync(x => x.MessageId == messageId && x.Consumer == consumer);
        }
    }
}