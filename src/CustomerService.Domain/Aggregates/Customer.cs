using CustomerService.Domain.Exceptions;
using CustomerService.Domain.ValueObjects;
using OrderService.Domain.ValueObjects;
using SharedKernel.ValueObjects;

namespace CustomerService.Domain.Aggregates;

public sealed class Customer
{
    public CustomerId Id { get; private set; } = null!;
    public CustomerName Name { get; private set; } = null!;
	public Email Email { get; private set; } = null!;
	public DateTime? EmailLastChangedAtUtc { get; private set; }
	public PhoneNumber? HomePhone { get; private set; }
    public PhoneNumber? MobilePhone { get; private set; }
    public bool IsActive { get; private set; }

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
		EmailLastChangedAtUtc = DateTime.UtcNow;
        IsActive = true;
    }

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

    public void UpdateContactDetails(
        Email newEmail,
        PhoneNumber? homePhone,
		PhoneNumber? mobilePhone,
		DateTime utcNow)
	{
		if(Email != newEmail)
		{
			if(EmailLastChangedAtUtc.HasValue)
			{
				var timeSinceLastChange = utcNow - EmailLastChangedAtUtc.Value;
				if (timeSinceLastChange < TimeSpan.FromDays(30))
				{
					throw new DomainException(
					  $"Email can only be changed once every 30 days. " +
					  $"Last changed {timeSinceLastChange.TotalDays:F0} days ago.");
				}
				
				Email = newEmail;
				EmailLastChangedAtUtc = utcNow;
			}
		}

        HomePhone = homePhone;
        MobilePhone = mobilePhone;
    }

    public void UpdateName(CustomerName newName)
	{
		Name = newName;
	}

    public void Activate()
	{
		// Send domain event to notify other bounded contexts eventually
        IsActive = true;
    }

    public void Deactivate()
	{
		// TODO: if customer has pending orders, customer can not be deactivated 
		// Send domain event to notify other bounded contexts eventually
        IsActive = false;
    }
}
