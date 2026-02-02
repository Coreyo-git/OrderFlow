namespace CustomerService.Application.DTOs;

public record CreateCustomerRequest(
	string Name,
	string Email,
	string? HomePhone,
	string? MobilePhone
);
