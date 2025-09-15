using OMS.Domain.Entities;

namespace OMS.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<bool> UpdateAsync(int id, Product updated);
        Task<bool> DeleteAsync(int id);
    }
}
