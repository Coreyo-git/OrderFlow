using CustomerService.Domain.Aggregates;
using CustomerService.Domain.Exceptions;
using FluentAssertions;
using OrderService.Domain.ValueObjects;
using SharedKernel.ValueObjects;

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
}
