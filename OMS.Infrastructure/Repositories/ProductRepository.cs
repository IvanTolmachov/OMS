using OMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OMS.Application.Interfaces;

namespace OMS.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OmsDbContext _db;

        public ProductRepository(OmsDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _db.Products.AsNoTracking().ToListAsync();

        public async Task<Product?> GetAsync(int id) =>
            await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> AddAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateAsync(int id, Product updated)
        {
            var existing = await _db.Products.FindAsync(id);
            if (existing == null) return false;
            existing.Name = updated.Name;
            existing.Price = updated.Price;
            existing.StockQuantity = updated.StockQuantity;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Products.FindAsync(id);
            if (existing == null) return false;
            _db.Products.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}