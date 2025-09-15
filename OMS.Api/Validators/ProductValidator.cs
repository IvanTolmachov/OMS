using FluentValidation;
using OMS.Api.Models;

namespace OMS.Api.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0).WithMessage("StockQuantity cannot be negative.");
        }
    }
}