using CustomerService.Application.DTOs;

namespace CustomerService.Application.Interfaces;

public interface ICustomerService
{
	Task<CustomerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<CustomerResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<CustomerResponse>> GetAllActiveAsync(CancellationToken cancellationToken = default);
	Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
}
