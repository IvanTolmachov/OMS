using Microsoft.AspNetCore.Mvc;
using OMS.Api.Models;
using OMS.Api.Services;
using FluentValidation;
using FluentValidation.Results;

namespace OMS.Api.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IValidator<OrderItemDto> _orderItemValidator;

        public OrdersController(IOrderService orderService, IValidator<OrderItemDto> orderItemValidator)
        {
            _orderService = orderService;
            _orderItemValidator = orderItemValidator;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] List<OrderItemDto> items)
        {
            foreach (var item in items)
            {
                ValidationResult validationResult = await _orderItemValidator.ValidateAsync(item);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.ToDictionary());
            }

            var (orderDto, error) = await _orderService.CreateOrderAsync(items);

            if (error != null)
                return BadRequest(error);

            return Ok(orderDto);
        }
    }
}