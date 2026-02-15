using FluentAssertions;
using OrderService.Domain.Aggregates;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Tests.Aggregates;

public class OrderTests
{
    private static Address CreateTestAddress(
        string street = "123 Main St",
        string city = "Anytown",
        string state = "CA",
        string zipCode = "90210",
        string country = "USA")
    {
        return Address.From(street, city, state, zipCode, country);
    }

    private static Product CreateTestProduct(
        ProductId? productId = null,
        string name = "Test Product",
        decimal price = 10.99m,
        string currency = "USD",
        string sku = "TESTSKU123")
    {
        return Product.Create(
            productId ?? ProductId.Create(),
            name,
            Money.From(currency, price),
            Sku.Create(sku));
    }

    private static Order CreateTestOrder(
        CustomerId? customerId = null,
        Address? shippingAddress = null,
        Address? billingAddress = null,
        IReadOnlyCollection<Product>? products = null)
    {
        var testCustomerId = customerId ?? CustomerId.From(Guid.NewGuid());
        var testShippingAddress = shippingAddress ?? CreateTestAddress();
        var testProducts = products ?? [CreateTestProduct()];

        return Order.Create(testCustomerId, testShippingAddress, billingAddress, testProducts);
    }

    [Fact]
    public void Should_create_order_with_valid_data_and_status_placed()
    {
        // Arrange
        var customerId = CustomerId.From(Guid.NewGuid());
        var shippingAddress = CreateTestAddress();
        var product = CreateTestProduct();
        var products = new List<Product> { product };

        // Act
        var order = Order.Create(customerId, shippingAddress, null, products);

        // Assert
        order.Should().NotBeNull();
        order.Id.Should().NotBeNull();
        order.CustomerId.Should().Be(customerId);
        order.Status.Should().Be(OrderStatus.Placed);
        order.ShippingAddress.Should().Be(shippingAddress);
        order.OrderItems.Should().HaveCount(1);
        order.OrderItems.First().ProductId.Should().Be(product.Id);
    }

    [Fact]
    public void Should_create_order_with_multiple_items()
    {
        // Arrange
        var customerId = CustomerId.From(Guid.NewGuid());
        var shippingAddress = CreateTestAddress();
        var product1 = CreateTestProduct(name: "Product A", price: 10m);
        var product2 = CreateTestProduct(name: "Product B", price: 20m);
        var products = new List<Product> { product1, product2 };

        // Act
        var order = Order.Create(customerId, shippingAddress, null, products);

        // Assert
        order.OrderItems.Should().HaveCount(2);
        order.OrderItems.Should().Contain(oi => oi.ProductId == product1.Id);
        order.OrderItems.Should().Contain(oi => oi.ProductId == product2.Id);
    }

    [Fact]
    public void Should_throw_exception_when_creating_order_with_no_products()
    {
        // Arrange
        var customerId = CustomerId.From(Guid.NewGuid());
        var shippingAddress = CreateTestAddress();
        var emptyProducts = new List<Product>();

        // Act
        Action act = () => Order.Create(customerId, shippingAddress, null, emptyProducts);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("An order must contain at least one item.");
    }

    [Fact]
    public void Should_confirm_order_successfully_from_placed_status()
    {
        // Arrange
        var order = CreateTestOrder(); // Status is Placed by default

        // Act
        order.ConfirmOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Confirmed);
    }

    [Fact]
    public void Should_not_confirm_order_if_not_placed()
    {
        // Arrange
        var order = CreateTestOrder();
        order.ConfirmOrder(); // Order is now Confirmed

        // Act
        Action act = () => order.ConfirmOrder();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Only a placed order can be confirmed.");
    }

    [Fact]
    public void Should_ship_order_successfully_from_confirmed_status()
    {
        // Arrange
        var order = CreateTestOrder();
        order.ConfirmOrder(); // Order is now Confirmed

        // Act
        order.ShipOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Shipped);
    }

    [Fact]
    public void Should_not_ship_order_if_not_confirmed()
    {
        // Arrange
        var order = CreateTestOrder(); // Order is Placed
        
        // Act
        Action act = () => order.ShipOrder();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Only a confirmed order can be shipped.");
    }

    [Fact]
    public void Should_complete_order_successfully_from_shipped_status()
    {
        // Arrange
        var order = CreateTestOrder();
        order.ConfirmOrder();
        order.ShipOrder(); // Order is now Shipped

        // Act
        order.CompleteOrder();

        // Assert
        order.Status.Should().Be(OrderStatus.Completed);
    }

    [Fact]
    public void Should_not_complete_order_if_not_shipped()
    {
        // Arrange
        var order = CreateTestOrder(); // Order is Placed
        
        // Act
        Action act = () => order.CompleteOrder();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Only a shipped order can be completed");
    }

    [Fact]
    public void Should_cancel_order_successfully_from_placed_or_confirmed()
    {
        // Arrange 1: Placed order
        var placedOrder = CreateTestOrder();

        // Act 1
        placedOrder.CancelOrder();

        // Assert 1
        placedOrder.Status.Should().Be(OrderStatus.Cancelled);

        // Arrange 2: Confirmed order
        var confirmedOrder = CreateTestOrder();
        confirmedOrder.ConfirmOrder();

        // Act 2
        confirmedOrder.CancelOrder();

        // Assert 2
        confirmedOrder.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void Should_not_cancel_order_if_shipped_or_completed()
    {
        // Arrange 1: Shipped order
        var shippedOrder = CreateTestOrder();
        shippedOrder.ConfirmOrder();
        shippedOrder.ShipOrder();

        // Act 1
        Action act1 = () => shippedOrder.CancelOrder();

        // Assert 1
        act1.Should().Throw<DomainException>().WithMessage($"Cannot cancel an order that is already {OrderStatus.Shipped}.");

        // Arrange 2: Completed order
        var completedOrder = CreateTestOrder();
        completedOrder.ConfirmOrder();
        completedOrder.ShipOrder();
        completedOrder.CompleteOrder();

        // Act 2
        Action act2 = () => completedOrder.CancelOrder();

        // Assert 2
        act2.Should().Throw<DomainException>().WithMessage($"Cannot cancel an order that is already {OrderStatus.Completed}.");
    }

    [Fact]
    public void Should_do_nothing_if_order_already_cancelled()
    {
        // Arrange
        var order = CreateTestOrder();
        order.CancelOrder(); // Cancel it once

        // Act
        order.CancelOrder(); // Try to cancel again

        // Assert
        order.Status.Should().Be(OrderStatus.Cancelled); // Should remain Cancelled
    }
}