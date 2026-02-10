using CustomerService.Domain.Aggregates;
using CustomerService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.SharedKernel.ValueObjects;
using OrderFlow.SharedKernel.ValueObjects;

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
			.HasConversion(
				name => name.Value,
				value => CustomerName.From(value)) 
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

		// TODO : Consider breaking apart Address into separate entities.
		builder.OwnsOne(c => c.BillingAddress, address =>
		{
			// AddressId value object conversion
			address.Property(a => a.Id)
				.HasColumnName("business_address_id")
				.HasConversion(
					id => id.Value,
					value => AddressId.From(value))  
				.IsRequired();

			address.Property(a => a.Street)
				.HasColumnName("billing_street")
				.HasMaxLength(200)
				.IsRequired();

			address.Property(a => a.City)
				.HasColumnName("billing_city")
				.HasMaxLength(100)
				.IsRequired();

			address.Property(a => a.State)
				.HasColumnName("billing_state")
				.HasMaxLength(100)
				.IsRequired();

			address.Property(a => a.PostalCode)
				.HasColumnName("billing_postal_code")
				.HasMaxLength(20)
				.IsRequired();

			address.Property(a => a.Country)
				.HasColumnName("billing_country")
				.HasMaxLength(100)
				.IsRequired();
		});

		// ShippingAddress - Owned Entity (complex type)
		builder.OwnsOne(c => c.ShippingAddress, address =>
		{
			// AddressId value object conversion
			address.Property(a => a.Id)
				.HasColumnName("shipping_address_id")
				.HasConversion(
					id => id.Value,
					value => AddressId.From(value))
				.IsRequired();

			address.Property(a => a.Street)
				.HasColumnName("shipping_street")
				.HasMaxLength(200)
				.IsRequired();

			address.Property(a => a.City)
				.HasColumnName("shipping_city")
				.HasMaxLength(100)
				.IsRequired();

			address.Property(a => a.State)
				.HasColumnName("shipping_state")
				.HasMaxLength(100)
				.IsRequired();

			address.Property(a => a.PostalCode)
				.HasColumnName("shipping_postal_code")
				.HasMaxLength(20)
				.IsRequired();

			address.Property(a => a.Country)
				.HasColumnName("shipping_country")
				.HasMaxLength(100)
				.IsRequired();
		});

		// Index for email lookups
		builder.HasIndex(c => c.Email).IsUnique();
		
		builder.HasIndex(c => c.IsActive)
			.HasDatabaseName("ix_customers_is_active");
	}
}
