using CustomerService.Domain.Aggregates;
using OrderFlow.SharedKernel.ValueObjects;
using SharedKernel.ValueObjects;

namespace CustomerService.Domain.Interfaces;

/// <summary>
/// Represents a repository for managing customers.
/// </summary>
public interface ICustomerRepository
{
	/// <summary>
	/// Gets a customer by their identifier.
	/// </summary>
	/// <param name="id">The customer identifier.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The customer if found; otherwise, <c>null</c>.</returns>
	Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a customer by their email address.
	/// </summary>
	/// <param name="email">The email address.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The customer if found; otherwise, <c>null</c>.</returns>
	Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a list of all customers, optionally filtered by their active status.
	/// </summary>
	/// <param name="isActive">The active status to filter by.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A read-only list of customers.</returns>
	Task<IReadOnlyList<Customer>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Adds a new customer to the repository.
	/// </summary>
	/// <param name="customer">The customer to add.</param>
	void Add(Customer customer);

	/// <summary>
	/// Saves any changes made to the repository.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task that represents the asynchronous save operation.</returns>
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
