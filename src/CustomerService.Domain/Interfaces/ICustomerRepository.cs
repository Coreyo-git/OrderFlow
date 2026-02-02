using CustomerService.Domain.Aggregates;
using CustomerService.Domain.ValueObjects;
using SharedKernel.ValueObjects;

namespace CustomerService.Domain.Interfaces;

public interface ICustomerRepository
{
	Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default);
	Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<Customer>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default);
	void Add(Customer customer);
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
