using OMS.Api.Models;
using OMS.Domain.Entities;
using AutoMapper;
using OMS.Application.Interfaces;

namespace OMS.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductAppService _appService;
        private readonly IMapper _mapper;

        public ProductService(IProductAppService appService, IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var domainProducts = await _appService.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(domainProducts);
        }

        public async Task<ProductDto?> GetAsync(int id)
        {
            var domainProduct = await _appService.GetAsync(id);
            return domainProduct == null ? null : _mapper.Map<ProductDto>(domainProduct);
        }

        public async Task<ProductDto> AddAsync(ProductDto product)
        {
            var domainProduct = _mapper.Map<Product>(product);
            var added = await _appService.AddAsync(domainProduct);
            return _mapper.Map<ProductDto>(added);
        }

        public async Task<bool> UpdateAsync(int id, ProductDto updated)
        {
            var domainProduct = _mapper.Map<Product>(updated);
            return await _appService.UpdateAsync(id, domainProduct);
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _appService.DeleteAsync(id);
    }
}