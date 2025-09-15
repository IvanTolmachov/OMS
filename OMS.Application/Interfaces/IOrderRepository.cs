using OMS.Domain.Entities;

namespace OMS.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetAsync(int id);
        Task<Order> AddAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersForDateAsync(DateTime date);
    }
}