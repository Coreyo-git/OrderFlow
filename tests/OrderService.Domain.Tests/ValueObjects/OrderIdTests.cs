using FluentAssertions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Tests.ValueObjects;

public class OrderIdTests
{
    [Fact]
    public void Should_create_new_order_id()
    {
        // Act
        var orderId1 = OrderId.Create();
        var orderId2 = OrderId.Create();

        // Assert
        orderId1.Should().NotBeNull();
        orderId1.Value.Should().NotBe(Guid.Empty);
        orderId1.Should().NotBe(orderId2);
    }

    [Fact]
    public void Should_create_order_id_from_valid_guid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var orderId = OrderId.From(guid);

        // Assert
        orderId.Should().NotBeNull();
        orderId.Value.Should().Be(guid);
    }

    [Fact]
    public void Should_throw_argument_exception_for_empty_guid()
    {
        // Act
        Action act = () => OrderId.From(Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("OrderId cannot be empty. (Parameter 'value')");
    }

    [Fact]
    public void Should_return_correct_string_representation()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var orderId = OrderId.From(guid);

        // Act
        var result = orderId.ToString();

        // Assert
        result.Should().Be(guid.ToString());
    }

    [Fact]
    public void Should_be_equal_when_guids_are_the_same()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var orderId1 = OrderId.From(guid);
        var orderId2 = OrderId.From(guid);

        // Assert
        orderId1.Should().Be(orderId2);
    }

    [Fact]
    public void Should_not_be_equal_when_guids_are_different()
    {
        // Arrange
        var orderId1 = OrderId.Create();
        var orderId2 = OrderId.Create();

        // Assert
        orderId1.Should().NotBe(orderId2);
    }
}
