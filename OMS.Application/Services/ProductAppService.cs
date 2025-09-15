using OMS.Application.Interfaces;
using OMS.Domain.Entities;

namespace OMS.Application.Services
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductRepository _repo;

        public ProductAppService(IProductRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Product>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Product?> GetAsync(int id) => _repo.GetAsync(id);
        public Task<Product> AddAsync(Product product) => _repo.AddAsync(product);
        public Task<bool> UpdateAsync(int id, Product updated) => _repo.UpdateAsync(id, updated);
        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}