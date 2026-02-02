namespace CustomerService.Application.DTOs;

public record CustomerResponse(
	Guid Id,
	string Name,
	string Email,
	string? HomePhone,
	string? MobilePhone,
	bool IsActive
);
