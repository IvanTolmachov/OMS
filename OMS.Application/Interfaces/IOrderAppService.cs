using OMS.Domain.Entities;

namespace OMS.Application.Interfaces
{
    public interface IOrderAppService
    {
        Task<(Order? order, string? error)> CreateOrderAsync(List<OrderItem> items);
        Task<IEnumerable<Order>> GetOrdersForDateAsync(DateTime date);
        // Add other methods as needed
    }
}