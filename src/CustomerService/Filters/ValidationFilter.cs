using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerService.Filters;

/// <summary>
/// Action filter that validates request bodies using FluentValidation.
/// Runs before the controller action and returns 400 Bad Request with
/// Problem Details if validation fails.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
	private readonly IServiceProvider _serviceProvider;

	public ValidationFilter(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		// Find arguments that might need validation (typically request DTOs)
		foreach (var argument in context.ActionArguments.Values)
		{
			if (argument is null)
				continue;

			var argumentType = argument.GetType();

			// Try to get a validator for this type
			var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
			var validator = _serviceProvider.GetService(validatorType) as IValidator;

			if (validator is null)
				continue;

			// Validate the argument
			var validationContext = new ValidationContext<object>(argument);
			var result = await validator.ValidateAsync(validationContext);

			if (!result.IsValid)
			{
				// Convert FluentValidation errors to Problem Details format
				var errors = result.Errors
					.GroupBy(e => e.PropertyName)
					.ToDictionary(
						g => g.Key,
						g => g.Select(e => e.ErrorMessage).ToArray());

				context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors)
				{
					Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
					Title = "Validation Failed",
					Status = StatusCodes.Status400BadRequest,
					Instance = context.HttpContext.Request.Path
				});

				return; // Short-circuit, don't execute the action
			}
		}

		// All validations passed, continue to the action
		await next();
	}
}
