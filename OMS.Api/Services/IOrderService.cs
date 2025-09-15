using OMS.Api.Models;

namespace OMS.Api.Services
{
    public interface IOrderService
    {
        Task<(OrderDto? orderDto, string? error)> CreateOrderAsync(List<OrderItemDto> items);
        Task<IEnumerable<OrderDto>> GetOrdersForDateAsync(DateTime date);
    }
}