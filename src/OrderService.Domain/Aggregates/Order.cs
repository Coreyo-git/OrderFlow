using OrderService.Domain.Enums;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Aggregates;

/// <summary>
/// Represents an order in the system.
/// </summary>
public sealed class Order
{
	private List<OrderItem> _orderItems = new();
	public OrderId Id { get; private set; }
	public CustomerId CustomerId { get; private set; }
	public OrderStatus Status { get; private set; }
	public IReadOnlyCollection<OrderItem> OrderItems => _orderItems; 

	private Order(CustomerId customerId)
	{
		Id = OrderId.Create();
		Status = OrderStatus.Draft;
		CustomerId = customerId;
	}

	/// <summary>
	/// Creates a new order.
	/// </summary>
	/// <param name="customerId">The customer identifier.</param>
	/// <returns>A new <see cref="Order"/> instance.</returns>
	public static Order Create(CustomerId customerId)
	{
		return new Order(customerId);
	}

	/// <summary>
	/// Updates the order status.
	/// </summary>
	/// <param name="newStatus">The new order status.</param>
	public void UpdateOrderStatus(OrderStatus newStatus)
	{
		// Add validation for transitions?
		Status = newStatus;
	}

	/// <summary>
	/// Adds a new item to the order.
	/// </summary>
	/// <param name="product">The product to add.</param>
	public void AddItem(Product product)
	{
		var orderItemToAdd = OrderItem.Create(Guid.NewGuid(), Id, product.Id, product.Price);

		_orderItems.Add(orderItemToAdd);
	}
}
