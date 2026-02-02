using CustomerService.Domain.Aggregates;
using CustomerService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.ValueObjects;

namespace CustomerService.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
	public void Configure(EntityTypeBuilder<Customer> builder)
	{
		builder.ToTable("customers");

		builder.HasKey(c => c.Id);

		// CustomerId value object conversion
		builder.Property(c => c.Id)
			.HasColumnName("id")
			.HasConversion(
				id => id.Value,                    // To database (Guid)
				value => CustomerId.From(value))   // From database (CustomerId)
			.IsRequired();

		builder.Property(c => c.Name)
			.HasColumnName("name")
			.HasMaxLength(200)
			.IsRequired();

		// Email value object conversion
		builder.Property(c => c.Email)
			.HasColumnName("email")
			.HasMaxLength(320)
			.HasConversion(
				email => email.Value,
				value => Email.From(value))
			.IsRequired();

		// Phone number conversions (nullable)
		builder.Property(c => c.HomePhone)
			.HasColumnName("home_phone")
			.HasMaxLength(15)
			.HasConversion(
				phone => phone == null ? null : phone.Value,
				value => value == null ? null : PhoneNumber.From(value));

		builder.Property(c => c.MobilePhone)
			.HasColumnName("mobile_phone")
			.HasMaxLength(15)
			.HasConversion(
				phone => phone == null ? null : phone.Value,
				value => value == null ? null : PhoneNumber.From(value));

		builder.Property(c => c.IsActive)
			.HasColumnName("is_active")
			.IsRequired();

		// Index for email lookups
		builder.HasIndex(c => c.Email).IsUnique();
	}
}
