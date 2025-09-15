using FluentValidation;
using OMS.Api.Models;

namespace OMS.Api.Validators
{
    public class OrderItemValidator : AbstractValidator<OrderItemDto>
    {
        public OrderItemValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("ProductId must be greater than zero.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be positive.");
        }
    }
}