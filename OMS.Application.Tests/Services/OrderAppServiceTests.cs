using Moq;
using OMS.Application.Interfaces;
using OMS.Application.Services;
using OMS.Domain.Entities;

public class OrderAppServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepoMock = new();
    private readonly Mock<IProductRepository> _productRepoMock = new();
    private readonly TimeProvider _timeProvider = TimeProvider.System;

    private OrderAppService CreateService()
        => new(_orderRepoMock.Object, _productRepoMock.Object, _timeProvider);

    [Fact]
    public async Task CreateOrderAsync_ReturnsError_WhenProductNotFound()
    {
        // Arrange
        _productRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product>());
        var service = CreateService();
        var items = new List<OrderItem> { new() { ProductId = 1, Quantity = 1 } };

        // Act
        var (order, error) = await service.CreateOrderAsync(items);

        // Assert
        Assert.Null(order);
        Assert.Equal("Product 1 not found.", error);
    }

    [Fact]
    public async Task CreateOrderAsync_ReturnsError_WhenQuantityIsNonPositive()
    {
        // Arrange
        var products = new List<Product> { new() { Id = 1, Name = "Test", Price = 10, StockQuantity = 5 } };
        _productRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
        var service = CreateService();
        var items = new List<OrderItem> { new() { ProductId = 1, Quantity = 0 } };

        // Act
        var (order, error) = await service.CreateOrderAsync(items);

        // Assert
        Assert.Null(order);
        Assert.Equal("Quantity for product 1 must be positive.", error);
    }

    [Fact]
    public async Task CreateOrderAsync_ReturnsError_WhenInsufficientStock()
    {
        // Arrange
        var products = new List<Product> { new() { Id = 1, Name = "Test", Price = 10, StockQuantity = 2 } };
        _productRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
        var service = CreateService();
        var items = new List<OrderItem> { new() { ProductId = 1, Quantity = 3 } };

        // Act
        var (order, error) = await service.CreateOrderAsync(items);

        // Assert
        Assert.Null(order);
        Assert.Equal("Insufficient stock for product Test.", error);
    }

    [Fact]
    public async Task CreateOrderAsync_SuccessfullyCreatesOrder_AndDeductsStock()
    {
        // Arrange
        var products = new List<Product> { new() { Id = 1, Name = "Test", Price = 10, StockQuantity = 5 } };
        _productRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
        _productRepoMock.Setup(r => r.UpdateAsync(1, It.IsAny<Product>())).ReturnsAsync(true);
        _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) =>
            {
                o.Id = 42;
                return o;
            });

        var service = CreateService();
        var items = new List<OrderItem> { new() { ProductId = 1, Quantity = 2 } };

        // Act
        var (order, error) = await service.CreateOrderAsync(items);

        // Assert
        Assert.NotNull(order);
        Assert.Null(error);
        Assert.Equal(42, order!.Id);
        Assert.Equal(20, order.TotalAmount);
        Assert.Single(order.Items);
        Assert.Equal(2, order.Items[0].Quantity);
        _productRepoMock.Verify(r => r.UpdateAsync(1, It.Is<Product>(p => p.StockQuantity == 3)), Times.Once);
        _orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task GetOrdersForDateAsync_ReturnsOrdersForGivenDate()
    {
        // Arrange
        var date = new DateTime(2024, 6, 1);
        var orders = new List<Order>
        {
            new() { Id = 1, Timestamp = new DateTimeOffset(date, TimeSpan.Zero), TotalAmount = 100 },
            new() { Id = 2, Timestamp = new DateTimeOffset(date, TimeSpan.Zero), TotalAmount = 200 }
        };
        _orderRepoMock.Setup(r => r.GetOrdersForDateAsync(date)).ReturnsAsync(orders);
        var service = CreateService();

        // Act
        var result = await service.GetOrdersForDateAsync(date);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, o => Assert.Equal(date, o.Timestamp.Date));
    }

    [Fact]
    public async Task GetOrdersForDateAsync_ReturnsEmpty_WhenNoOrdersForDate()
    {
        // Arrange
        var date = new DateTime(2024, 6, 2);
        _orderRepoMock.Setup(r => r.GetOrdersForDateAsync(date)).ReturnsAsync(new List<Order>());
        var service = CreateService();

        // Act
        var result = await service.GetOrdersForDateAsync(date);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}