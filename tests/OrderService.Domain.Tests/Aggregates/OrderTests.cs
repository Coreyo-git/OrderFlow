using FluentAssertions;
using OrderService.Domain.Aggregates;
using SharedKernel.ValueObjects;
using OrderService.Domain.Enums; // Add this for OrderStatus
using OrderService.Domain.ValueObjects; // Add this for ValueObjects

namespace OrderService.Domain.Tests.Aggregates;

public class OrderTests()
{
	private static Order CreateTestOrder()
	{
		return Order.Create(CustomerId.From(Guid.NewGuid()));
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

    private static OrderItem CreateTestOrderItem(
        Guid? orderItemId = null,
        OrderId? orderId = null,
        ProductId? productId = null,
        decimal price = 10.99m,
        string currency = "AUD")
    {
        return OrderItem.Create(
            orderItemId ?? Guid.NewGuid(),
            orderId ?? OrderId.From(Guid.NewGuid()),
            productId ?? ProductId.Create(),
            Money.From(currency, price));
    }

	[Fact]
	public void Should_create_order_with_valid_data()
	{
		// Arrange
		var customer_id = CustomerId.From(Guid.NewGuid());

		// Act
		var order = Order.Create(customer_id);

		// Assert
		order.Status.Should().Be(OrderStatus.Draft);
	}

	[Fact]
	public void Should_create_order_with_valid_order_items()
	{
		// Arrange
		var customer_id = CustomerId.From(Guid.NewGuid());
		var order = Order.Create(customer_id);
		var product_id = ProductId.From(Guid.NewGuid());
		var product_price = 15.50m;
		var product = CreateTestProduct(productId: product_id, price: product_price);

		// Act
		order.AddItem(product);

		// Assert
		order.OrderItems.Should().HaveCount(1);
		order.OrderItems.Should().ContainSingle(oi =>
			oi.ProductId == product_id &&
			oi.Price.Quantity == product_price);
	}

	[Fact]
	public void Should_update_order_status_correctly()
	{
		// Arrange
		var order = CreateTestOrder(); // Order is in Draft status initially

		// Act
		order.UpdateOrderStatus(OrderStatus.Placed);

		// Assert
		order.Status.Should().Be(OrderStatus.Placed);
	}

	[Fact]
	public void Should_add_multiple_distinct_items_to_order()
	{
		// Arrange
		var order = CreateTestOrder();
		var product1_id = ProductId.From(Guid.NewGuid());
		var product1_price = 10.00m;
		var product1 = CreateTestProduct(productId: product1_id, name: "Product 1", price: product1_price);

		var product2_id = ProductId.From(Guid.NewGuid());
		var product2_price = 20.00m;
		var product2 = CreateTestProduct(productId: product2_id, name: "Product 2", price: product2_price);

		// Act
		order.AddItem(product1);
		order.AddItem(product2);

		// Assert
		order.OrderItems.Should().HaveCount(2);
		order.OrderItems.Should().Contain(oi => oi.ProductId == product1_id && oi.Price.Quantity == product1_price);
		order.OrderItems.Should().Contain(oi => oi.ProductId == product2_id && oi.Price.Quantity == product2_price);
	}

	[Fact]
	public void Should_add_the_same_product_multiple_times()
	{
		// Arrange
		var order = CreateTestOrder();
		var product_id = ProductId.From(Guid.NewGuid());
		var product_price = 5.00m;
		var product = CreateTestProduct(productId: product_id, name: "Single Product", price: product_price);

		// Act
		order.AddItem(product);
		order.AddItem(product);

		// Assert
		order.OrderItems.Should().HaveCount(2);
		order.OrderItems.Should().AllSatisfy(oi =>
		{
			oi.ProductId.Should().Be(product_id);
			oi.Price.Quantity.Should().Be(product_price);
		});
	}
}