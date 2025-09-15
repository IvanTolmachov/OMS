using OMS.Application.Interfaces;
using OMS.Domain.Entities;

namespace OMS.Application.Services
{
    public class OrderAppService : IOrderAppService
    {
        private readonly IOrderRepository _repo;
        private readonly IProductRepository _productRepo;
        private readonly TimeProvider _timeProvider;

        public OrderAppService(IOrderRepository repo, IProductRepository productRepo, TimeProvider timeProvider)
        {
            _repo = repo;
            _productRepo = productRepo;
            _timeProvider = timeProvider;
        }

        public async Task<(Order? order, string? error)> CreateOrderAsync(List<OrderItem> items)
        {
            // TODO : Use a transaction to ensure atomicity

            var products = (await _productRepo.GetAllAsync()).ToDictionary(p => p.Id);

            // Validate all items before processing
            foreach (var item in items)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                    return (null, $"Product {item.ProductId} not found.");

                if (item.Quantity <= 0)
                    return (null, $"Quantity for product {item.ProductId} must be positive.");

                if (product.StockQuantity < item.Quantity)
                    return (null, $"Insufficient stock for product {product.Name}.");
            }

            // Calculate total and prepare order items 
            var total = items
                .Sum(i => products[i.ProductId].Price * i.Quantity);

            var orderItems = items
                .Select(i => new OrderItem { ProductId = i.ProductId, Quantity = i.Quantity })
                .ToList();

            // Deduct stock 
            foreach (var item in items)
            {
                var product = products[item.ProductId];
                var updatedProduct = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity - item.Quantity
                };
                await _productRepo.UpdateAsync(product.Id, updatedProduct);
            }

            var order = new Order
            {
                Timestamp = _timeProvider.GetUtcNow(),
                Items = orderItems,
                TotalAmount = total
            };

            var addedOrder = await _repo.AddAsync(order);
            return (addedOrder, null);
        }

        public Task<IEnumerable<Order>> GetOrdersForDateAsync(DateTime date) =>
            _repo.GetOrdersForDateAsync(date);
    }
}