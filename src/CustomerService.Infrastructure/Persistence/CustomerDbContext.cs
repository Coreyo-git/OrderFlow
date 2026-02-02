using CustomerService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence;

public class CustomerDbContext : DbContext
{
	public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
		: base(options) { }

	public DbSet<Customer> Customers => Set<Customer>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
	}
}
