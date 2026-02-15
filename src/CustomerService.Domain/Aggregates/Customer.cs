using CustomerService.Domain.Exceptions;
using CustomerService.Domain.ValueObjects;
using OrderFlow.SharedKernel.ValueObjects;
using SharedKernel.ValueObjects;

namespace CustomerService.Domain.Aggregates;

/// <summary>
/// Represents a customer in the system.
/// </summary>
public sealed class Customer
{
	public CustomerId Id { get; private set; } = null!;
	public CustomerName Name { get; private set; } = null!;
	public Email Email { get; private set; } = null!;
	public DateTime? EmailLastChangedAtUtc { get; private set; }
	public PhoneNumber? HomePhone { get; private set; }
	public PhoneNumber? MobilePhone { get; private set; }
	public bool IsActive { get; private set; }
	public Address? BillingAddress { get; private set; }
	public Address? ShippingAddress { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Customer"/> class.
	/// </summary>
	/// <remarks>
	/// Required by EF Core.
	/// </remarks>
	private Customer() { } // EF Core

	private Customer(
		 CustomerId id,
		 CustomerName name,
		 Email email,
		 PhoneNumber? homePhone,
		 PhoneNumber? mobilePhone)
	{
		Id = id;
		Name = name;
		Email = email;
		HomePhone = homePhone;
		MobilePhone = mobilePhone;
		IsActive = true;
	}

	/// <summary>
	/// Creates a new customer.
	/// </summary>
	/// <param name="name">The customer's name.</param>
	/// <param name="email">The customer's email address.</param>
	/// <param name="homePhone">The customer's home phone number.</param>
	/// <param name="mobilePhone">The customer's mobile phone number.</param>
	/// <returns>A new <see cref="Customer"/> instance.</returns>
	public static Customer Create(
		CustomerName name,
		Email email,
		PhoneNumber? homePhone = null,
		PhoneNumber? mobilePhone = null)
	{

		return new Customer(
			CustomerId.Create(),
			name,
			email,
			homePhone,
			mobilePhone);
	}

	/// <summary>
	/// Updates the customer's contact details.
	/// </summary>
	/// <param name="newEmail">The new email address.</param>
	/// <param name="homePhone">The new home phone number.</param>
	/// <param name="mobilePhone">The new mobile phone number.</param>
	/// <param name="utcNow">The current UTC date and time.</param>
	/// <exception cref="DomainException">Thrown when email is changed more than once every 30 days.</exception>
	public void UpdateContactDetails(
		Email newEmail,
		PhoneNumber? homePhone,
		PhoneNumber? mobilePhone,
		DateTime utcNow)
	{
		if (Email != newEmail)
		{
			// Only enforce 30-day rule if email was previously changed
			if (EmailLastChangedAtUtc.HasValue)
			{
				var timeSinceLastChange = utcNow - EmailLastChangedAtUtc.Value;
				if (timeSinceLastChange < TimeSpan.FromDays(30))
				{
					throw new DomainException(
						$"Email can only be changed once every 30 days. " +
						$"Last changed {timeSinceLastChange.TotalDays:F0} days ago.");
				}
			}

			Email = newEmail;
			EmailLastChangedAtUtc = utcNow;
		}

		HomePhone = homePhone;
		MobilePhone = mobilePhone;
	}

	/// <summary>
	/// Updates the customer's name.
	/// </summary>
	/// <param name="newName">The new name.</param>
	public void UpdateName(CustomerName newName)
	{
		Name = newName;
	}

	/// <summary>
	/// Updates the customer's address details.
	/// </summary>
	/// <param name="billingAddress">The new billing address.</param>
	/// <param name="shippingAddress">The new shipping address.</param>
	public void UpdateAddressDetails(Address? billingAddress, Address? shippingAddress)
	{
		BillingAddress = billingAddress;
		ShippingAddress = shippingAddress;
	}

	/// <summary>
	/// Activates the customer.
	/// </summary>
	public void Activate()
	{
		// Send domain event to notify other bounded contexts eventually
		IsActive = true;
	}

	/// <summary>
	/// Deactivates the customer.
	/// </summary>
	public void Deactivate()
	{
		// TODO: if customer has pending orders, customer can not be deactivated 
		// Send domain event to notify other bounded contexts eventually
		IsActive = false;
	}
}
