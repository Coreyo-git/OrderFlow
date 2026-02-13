using System.Data.SqlTypes;
using OrderService.Domain.Aggregates;

namespace OrderService.Domain.ValueObjects;
public sealed record OrderItem
{
	public Guid Id { get; private set;}
	public OrderId OrderId { get; private set; }
	public ProductId ProductId { get; private set; }
	public Money Price { get; private set; }

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