using CustomerService.Domain.Aggregates;
using CustomerService.Domain.Exceptions;
using CustomerService.Domain.ValueObjects;
using FluentAssertions;
using OrderFlow.SharedKernel.ValueObjects;

namespace CustomerService.Domain.Tests.Aggregates;

public class CustomerTests
{
    // Helper to create a valid customer for testing
    private static Customer CreateTestCustomer(string email = "test@example.com")
    {
        return Customer.Create(
            CustomerName.From("Test Customer"),
            Email.From(email));
    }

    public class Create
    {
        [Fact]
        public void Should_create_customer_with_valid_data()
        {
            // Arrange
            var name = CustomerName.From("John Doe");
            var email = Email.From("john@example.com");

            // Act
            var customer = Customer.Create(name, email);

            // Assert
            customer.Name.Should().Be(name);
            customer.Email.Should().Be(email);
        }

        [Fact]
        public void Should_set_IsActive_to_true_on_creation()
        {
            // Act
            var customer = CreateTestCustomer();

            // Assert
            customer.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Should_assign_new_id_on_creation()
        {
            // Act
            var customer = CreateTestCustomer();

            // Assert
            customer.Id.Should().NotBeNull();
            customer.Id.Value.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void Should_set_phones_to_null_when_not_provided()
        {
            // Act
            var customer = CreateTestCustomer();

            // Assert
            customer.HomePhone.Should().BeNull();
            customer.MobilePhone.Should().BeNull();
        }

        [Fact]
        public void Should_set_phones_when_provided()
        {
            // Arrange
            var homePhone = PhoneNumber.From("12345678");
            var mobilePhone = PhoneNumber.From("87654321");

            // Act
            var customer = Customer.Create(
                CustomerName.From("Test"),
                Email.From("test@example.com"),
                homePhone,
                mobilePhone);

            // Assert
            customer.HomePhone.Should().Be(homePhone);
            customer.MobilePhone.Should().Be(mobilePhone);
        }
    }

    public class UpdateName
    {
        [Fact]
        public void Should_update_name()
        {
            // Arrange
            var customer = CreateTestCustomer();
            var newName = CustomerName.From("New Name");

            // Act
            customer.UpdateName(newName);

            // Assert
            customer.Name.Should().Be(newName);
        }
    }

    public class Activate
    {
        [Fact]
        public void Should_set_IsActive_to_true()
        {
            // Arrange
            var customer = CreateTestCustomer();
            customer.Deactivate(); // First deactivate

            // Act
            customer.Activate();

            // Assert
            customer.IsActive.Should().BeTrue();
        }
    }

    public class Deactivate
    {
        [Fact]
        public void Should_set_IsActive_to_false()
        {
            // Arrange
            var customer = CreateTestCustomer();

            // Act
            customer.Deactivate();

            // Assert
            customer.IsActive.Should().BeFalse();
        }
    }

    public class UpdateContactDetails
    {
        [Fact]
        public void Should_update_email_on_first_change_after_creation()
        {
            // Arrange
            var customer = CreateTestCustomer("original@example.com");
            var newEmail = Email.From("updated@example.com");
            var now = DateTime.UtcNow;

			// Act
			customer.UpdateContactDetails(newEmail, null, null, now);

            // Assert
            customer.Email.Should().Be(newEmail);
        }

        [Fact]
        public void Should_throw_when_email_changed_within_30_days()
        {
            // Arrange
            var customer = CreateTestCustomer("original@example.com");
            var firstChange = Email.From("first@example.com");
            var secondChange = Email.From("second@example.com");
            var now = DateTime.UtcNow;

            // First change should work
            customer.UpdateContactDetails(firstChange, null, null, now);

            // Act & Assert - second change 29 days later should fail
            var action = () => customer.UpdateContactDetails(
                secondChange, null, null, now.AddDays(29));

            action.Should().Throw<DomainException>()
                .WithMessage("*30 days*");
        }

        [Fact]
        public void Should_allow_email_change_after_30_days()
        {
            // Arrange
            var customer = CreateTestCustomer("original@example.com");
            var firstChange = Email.From("first@example.com");
            var secondChange = Email.From("second@example.com");
            var now = DateTime.UtcNow;

            customer.UpdateContactDetails(firstChange, null, null, now);

            // Act - change after 31 days should succeed
            customer.UpdateContactDetails(secondChange, null, null, now.AddDays(31));

            // Assert
            customer.Email.Should().Be(secondChange);
        }

        [Fact]
        public void Should_not_throw_when_email_is_unchanged()
        {
            // Arrange
            var customer = CreateTestCustomer("same@example.com");
            var sameEmail = Email.From("same@example.com");
            var now = DateTime.UtcNow;

            // Act - updating with same email should never throw
            var action = () => customer.UpdateContactDetails(sameEmail, null, null, now);

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Should_always_update_phone_numbers()
        {
            // Arrange
            var customer = CreateTestCustomer();
            var homePhone = PhoneNumber.From("12345678");
            var mobilePhone = PhoneNumber.From("87654321");

            // Act
            customer.UpdateContactDetails(
                customer.Email, homePhone, mobilePhone, DateTime.UtcNow);

            // Assert - phone updates should always work regardless of email rules
            customer.HomePhone.Should().Be(homePhone);
            customer.MobilePhone.Should().Be(mobilePhone);
        }
    }
	public class UpdateAddressDetails()
	{
		[Fact]
		public void Should_update_billing_and_shipping_addresses()
		{
			// Arrange
			var customer = CreateTestCustomer();
			var billingAddress = Address.From(
				"123 Billing St", "Billtown", "State", "12345", "Country");
			var shippingAddress = Address.From(
				"456 Shipping Ave", "Shipville", "State", "67890", "Country");

			// Act
			customer.UpdateAddressDetails(billingAddress, shippingAddress);

			// Assert
			customer.BillingAddress.Should().Be(billingAddress);
			customer.ShippingAddress.Should().Be(shippingAddress);
		}

		[Fact]
		public void Should_update_billing_address_only()
		{
			// Arrange
			var customer = CreateTestCustomer();
			var billingAddress = Address.From(
				"123 Billing St", "Billtown", "State", "12345", "Country");

			// Act
			customer.UpdateAddressDetails(billingAddress, null);

			// Assert
			customer.BillingAddress.Should().Be(billingAddress);
			customer.ShippingAddress.Should().Be(null);
		}

		[Fact]
		public void Should_update_shipping_address_only()
		{
			// Arrange
			var customer = CreateTestCustomer();
			var shippingAddress = Address.From(
				"456 Shipping Ave", "Shipville", "State", "67890", "Country");

			// Act
			customer.UpdateAddressDetails(null, shippingAddress);

			// Assert
			customer.BillingAddress.Should().Be(null);
			customer.ShippingAddress.Should().Be(shippingAddress);
		}

		[Fact]
		public void Should_allow_clearing_addresses()
		{
			// Arrange
			var customer = CreateTestCustomer();
			var billingAddress = Address.From(
				"123 Billing St", "Billtown", "State", "12345", "Country");
			var shippingAddress = Address.From(
				"456 Shipping Ave", "Shipville", "State", "67890", "Country");

			customer.UpdateAddressDetails(billingAddress, shippingAddress);

			// Act - clear both addresses
			customer.UpdateAddressDetails(null, null);

			// Assert
			customer.BillingAddress.Should().Be(null);
			customer.ShippingAddress.Should().Be(null);
		}
	}
}
