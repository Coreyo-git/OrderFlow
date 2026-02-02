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

	public async Task<IReadOnlyList<Customer>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default)
	{
		var query = _context.Customers.AsQueryable();

		if (isActive.HasValue)
			query = query.Where(c => c.IsActive == isActive.Value);

		return await query.ToListAsync(cancellationToken);
	}

	public void Add(Customer customer)
	{
		_context.Customers.Add(customer);
	}

	public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await _context.SaveChangesAsync(cancellationToken);
	}
}
