using CustomerService.Domain.Aggregates;
using CustomerService.Domain.Interfaces;
using CustomerService.Domain.ValueObjects;
using CustomerService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.ValueObjects;

namespace CustomerService.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
	private readonly CustomerDbContext _context;

	public CustomerRepository(CustomerDbContext context)
	{
		_context = context;
	}

	public async Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default)
	{
		return await _context.Customers
			.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
	}

	public async Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
	{
		return await _context.Customers
			.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
	}

	public async Task<IReadOnlyList<Customer>> GetAllAsync(bool isActive = false, CancellationToken cancellationToken = default)
	{
		return await _context.Customers
			.Where(c => c.IsActive == isActive)
			.ToListAsync(cancellationToken);
	}

	public void Add(Customer customer)
	{
		_context.Customers.Add(customer);
	}

	public void Update(Customer customer)
	{
		_context.Customers.Update(customer);
	}

	public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await _context.SaveChangesAsync(cancellationToken);
	}
}
