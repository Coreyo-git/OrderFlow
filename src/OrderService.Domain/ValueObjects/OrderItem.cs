using System.Data.SqlTypes;
using OrderService.Domain.Aggregates;

namespace OrderService.Domain.ValueObjects;

/// <summary>
/// Represents an item within an order.
/// </summary>
public sealed record OrderItem
{
	public Guid Id { get; private set;}
	public OrderId OrderId { get; private set; }
	public ProductId ProductId { get; private set; }
	public Money Price { get; private set; }

	/// <summary>
	/// Creates a new <see cref="OrderItem"/>.
	/// </summary>
	/// <param name="id">The unique identifier of the order item.</param>
	/// <param name="orderId">The identifier of the order this item belongs to.</param>
	/// <param name="productId">The identifier of the product.</param>
	/// <param name="price">The price of the product at the time of order.</param>
	/// <returns>A new <see cref="OrderItem"/> instance.</returns>
	public static OrderItem Create(Guid id, OrderId orderId, ProductId productId, Money price)
	{
		// handle validation here or maybe remove later in refactor
		return new OrderItem(id, orderId, productId, price);
	}

	private OrderItem(Guid id, OrderId orderId, ProductId productId, Money price)
	{
		Id = id;
		OrderId = orderId;
		ProductId = productId;
		Price = price;
	}
}