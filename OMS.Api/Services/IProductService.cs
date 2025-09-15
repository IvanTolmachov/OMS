using OMS.Api.Models;

namespace OMS.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetAsync(int id);
        Task<ProductDto> AddAsync(ProductDto product);
        Task<bool> UpdateAsync(int id, ProductDto updated);
        Task<bool> DeleteAsync(int id);
    }
}