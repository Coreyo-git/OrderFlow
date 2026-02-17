using FluentAssertions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Tests.ValueObjects;

public class CustomerIdTests
{
    [Fact]
    public void Should_create_customer_id_from_valid_guid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var customerId = CustomerId.From(guid);

        // Assert
        customerId.Should().NotBeNull();
        customerId.Value.Should().Be(guid);
    }

    [Fact]
    public void Should_throw_argument_exception_for_empty_guid()
    {
        // Act
        Action act = () => CustomerId.From(Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("CustomerId cannot be empty. (Parameter 'value')");
    }

    [Fact]
    public void Should_return_correct_string_representation()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var customerId = CustomerId.From(guid);

        // Act
        var result = customerId.ToString();

        // Assert
        result.Should().Be(guid.ToString());
    }

    [Fact]
    public void Should_be_equal_when_guids_are_the_same()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var customerId1 = CustomerId.From(guid);
        var customerId2 = CustomerId.From(guid);

        // Assert
        customerId1.Should().Be(customerId2);
        (customerId1 == customerId2).Should().BeTrue();
    }

    [Fact]
    public void Should_not_be_equal_when_guids_are_different()
    {
        // Arrange
        var customerId1 = CustomerId.From(Guid.NewGuid());
        var customerId2 = CustomerId.From(Guid.NewGuid());

        // Assert
        customerId1.Should().NotBe(customerId2);
        (customerId1 != customerId2).Should().BeTrue();
    }
}
