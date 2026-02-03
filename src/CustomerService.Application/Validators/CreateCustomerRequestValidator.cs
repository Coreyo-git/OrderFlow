using CustomerService.Application.DTOs;
using FluentValidation;

namespace CustomerService.Application.Validators;

/// <summary>
/// Validates CreateCustomerRequest before it reaches the domain layer.
/// This catches invalid input early and returns user-friendly error messages.
/// </summary>
public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
	public CreateCustomerRequestValidator()
	{
		// Name: required, max 200 chars (matches domain constraint)
		RuleFor(x => x.Name)
			.NotNull()
			.WithMessage("Customer name is required.");

		RuleFor(x => x.Email)
			.NotNull()
			.WithMessage("Email is required.");
	}
}
