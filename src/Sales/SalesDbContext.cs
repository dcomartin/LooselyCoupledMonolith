using System;
using Microsoft.EntityFrameworkCore;

namespace Sales
{
    public class SalesDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLExpress;Database=Demo;Trusted_Connection=Yes;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}