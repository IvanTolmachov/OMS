using AutoMapper;
using OMS.Api.Models;
using OMS.Application.Interfaces;
using OMS.Domain.Entities;

namespace OMS.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderAppService _appService;
        private readonly IMapper _mapper;

        public OrderService(IOrderAppService appService, IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }

        public async Task<(OrderDto? orderDto, string? error)> CreateOrderAsync(List<OrderItemDto> items)
        {
            var domainItems = _mapper.Map<List<OrderItem>>(items);

            var (order, error) = await _appService.CreateOrderAsync(domainItems);

            if (order == null)
                return (null, error);
            
            var orderDto = _mapper.Map<OrderDto>(order);
            
            return (orderDto, null);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersForDateAsync(DateTime date)
        {
            var domainOrders = await _appService.GetOrdersForDateAsync(date);
            return _mapper.Map<IEnumerable<OrderDto>>(domainOrders);
        }
    }
}