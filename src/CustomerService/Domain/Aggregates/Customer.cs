using CustomerService.Domain.ValueObjects;
using SharedKernel.ValueObjects;

namespace CustomerService.Domain.Aggregates;

public sealed class Customer
{
    public CustomerId Id { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public PhoneNumber? HomePhone { get; private set; }
    public PhoneNumber? MobilePhone { get; private set; }
    public bool IsActive { get; private set; }

    private Customer() { } // EF Core

    private Customer(
        CustomerId id,
        string name,
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

    public static Customer Create(
        string name,
        Email email,
        PhoneNumber? homePhone = null,
        PhoneNumber? mobilePhone = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty.", nameof(name));

        return new Customer(
            CustomerId.Create(),
            name.Trim(),
            email,
            homePhone,
            mobilePhone);
    }

    public void UpdateContactDetails(
        Email email,
        PhoneNumber? homePhone,
        PhoneNumber? mobilePhone)
    {
        Email = email;
        HomePhone = homePhone;
        MobilePhone = mobilePhone;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty.", nameof(name));

        Name = name.Trim();
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
