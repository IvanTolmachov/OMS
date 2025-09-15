using Microsoft.AspNetCore.Mvc;
using OMS.Api.Services;

namespace OMS.Api.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public ReportsController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        [HttpGet("daily-summary")]
        public async Task<IActionResult> DailySummary()
        {
            var todayOrders = await _orderService.GetOrdersForDateAsync(DateTime.UtcNow);
            var totalOrders = todayOrders.Count();
            var totalRevenue = todayOrders.Sum(o => o.TotalAmount);
            return Ok(new { totalOrders, totalRevenue });
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> LowStock()
        {
            var products = await _productService.GetAllAsync();
            var lowStock = products.Where(p => p.StockQuantity < 5);
            return Ok(lowStock);
        }
    }
}