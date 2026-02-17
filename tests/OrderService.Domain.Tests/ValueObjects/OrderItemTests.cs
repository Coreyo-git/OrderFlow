using FluentAssertions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Tests.ValueObjects;

public class OrderItemTests
{
    [Fact]
    public void Should_create_order_item_with_valid_data()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = OrderId.Create();
        var productId = ProductId.Create();
        var price = Money.From("USD", 10.99m);

        // Act
        var orderItem = OrderItem.Create(id, orderId, productId, price);

        // Assert
        orderItem.Should().NotBeNull();
        orderItem.Id.Should().Be(id);
        orderItem.OrderId.Should().Be(orderId);
        orderItem.ProductId.Should().Be(productId);
        orderItem.Price.Should().Be(price);
    }
    
    [Fact]
    public void Should_be_equal_when_all_properties_are_the_same()
    {
        // Arrange
        var id = Guid.NewGuid();
        var orderId = OrderId.Create();
        var productId = ProductId.Create();
        var price = Money.From("USD", 25.50m);

        var orderItem1 = OrderItem.Create(id, orderId, productId, price);
        var orderItem2 = OrderItem.Create(id, orderId, productId, price);

        // Assert
        orderItem1.Should().Be(orderItem2);
        (orderItem1 == orderItem2).Should().BeTrue();
    }

    [Fact]
    public void Should_not_be_equal_if_id_is_different()
    {
        // Arrange
        var orderId = OrderId.Create();
        var productId = ProductId.Create();
        var price = Money.From("USD", 25.50m);

        var orderItem1 = OrderItem.Create(Guid.NewGuid(), orderId, productId, price);
        var orderItem2 = OrderItem.Create(Guid.NewGuid(), orderId, productId, price);

        // Assert
        orderItem1.Should().NotBe(orderItem2);
    }

    [Fact]
    public void Should_not_be_equal_if_order_id_is_different()
    {
        // Arrange
        var id = Guid.NewGuid();
        var productId = ProductId.Create();
        var price = Money.From("USD", 25.50m);

        var orderItem1 = OrderItem.Create(id, OrderId.Create(), productId, price);
        var orderItem2 = OrderItem.Create(id, OrderId.Create(), productId, price);

        // Assert
        orderItem1.Should().NotBe(orderItem2);
    }
}
