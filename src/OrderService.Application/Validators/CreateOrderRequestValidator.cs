using FluentValidation;

using OrderService.Application.DTOs;

namespace OrderService.Application.Validators;

/// <summary>
/// Validates CreateOrderRequest before it reaches the domain layer.
/// This catches invalid input early and returns user-friendly error messages.
/// </summary>
public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer id is required.");

        RuleFor(x => x.ShippingAddress)
            .NotNull()
            .WithMessage("Shipping address is required.");

        RuleFor(x => x.Culture)
            .NotEmpty()
            .WithMessage("Culture is required.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("An order must contain at least one item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId).NotEmpty().WithMessage("Item product id is required.");
            item.RuleFor(i => i.ProductName).NotEmpty().WithMessage("Item product name is required.");
            item.RuleFor(i => i.Sku).NotEmpty().WithMessage("Item SKU is required.");
            item.RuleFor(i => i.Price).GreaterThan(0).WithMessage("Item price must be greater than 0.");
            item.RuleFor(i => i.Currency).NotEmpty().WithMessage("Item currency is required.");
        });
    }
}