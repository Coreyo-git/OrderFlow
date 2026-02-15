using CustomerService.Application.DTOs;
using CustomerService.Application.Interfaces;
using CustomerService.Domain.Aggregates;
using CustomerService.Domain.Interfaces;
using CustomerService.Domain.ValueObjects;

namespace CustomerService.Application.Services;

public class CustomerApplicationService : ICustomerService
{
	private readonly ICustomerRepository _customerRepository;

	public CustomerApplicationService(ICustomerRepository customerRepository)
	{
		_customerRepository = customerRepository;
	}

	public async Task<CustomerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var customer = await _customerRepository.GetByIdAsync(CustomerId.From(id), cancellationToken);
		return customer is null ? null : MapToResponse(customer);
	}

	public async Task<CustomerResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		var customer = await _customerRepository.GetByEmailAsync(Email.From(email), cancellationToken);
		return customer is null ? null : MapToResponse(customer);
	}

	public async Task<IReadOnlyList<CustomerResponse>> GetAllActiveAsync(CancellationToken cancellationToken = default)
	{
		var customers = await _customerRepository.GetAllAsync(isActive: true, cancellationToken);
		return customers.Select(MapToResponse).ToList();
	}

	public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
	{
		var customer = Customer.Create(
			CustomerName.From(request.Name),
			Email.From(request.Email),
			PhoneNumber.FromNullable(request.HomePhone),
			PhoneNumber.FromNullable(request.MobilePhone));

		_customerRepository.Add(customer);
		await _customerRepository.SaveChangesAsync(cancellationToken);

		return MapToResponse(customer);
	}

	private static CustomerResponse MapToResponse(Customer customer)
	{
		return new CustomerResponse(
			customer.Id.Value,
			customer.Name.Value,
			customer.Email.Value,
			customer.HomePhone?.Value,
			customer.MobilePhone?.Value,
			customer.IsActive);
	}
}