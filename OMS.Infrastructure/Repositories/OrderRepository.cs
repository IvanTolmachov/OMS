using OMS.Application.Interfaces;
using OMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace OMS.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OmsDbContext _db;

        public OrderRepository(OmsDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Order>> GetAllAsync() =>
            await _db.Orders.AsNoTracking().ToListAsync();

        public async Task<Order?> GetAsync(int id) =>
            await _db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

        public async Task<Order> AddAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersForDateAsync(DateTime date)
        {
            return await _db.Orders
                .AsNoTracking()
                .Where(o => o.Timestamp.Date == date.Date)
                .ToListAsync();
        }

        // Add other methods as needed
    }
}