using Microsoft.EntityFrameworkCore;
using OMS.Domain.Entities;

namespace OMS.Infrastructure
{
    public class OmsDbContext : DbContext
    {
        public OmsDbContext(DbContextOptions<OmsDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
    }
}