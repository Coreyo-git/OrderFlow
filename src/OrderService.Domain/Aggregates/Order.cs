using OrderService.Domain.Enums;
using OrderService.Domain.ValueObjects;
using OrderFlow.SharedKernel.ValueObjects;
using SharedKernel.ValueObjects;
using System.Collections.Generic;

namespace OrderService.Domain.Aggregates;

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

	public static Order Create(CustomerId customerId)
	{
		return new Order(customerId);
	}

	public void UpdateOrderStatus(OrderStatus newStatus)
	{
		// Add validation for transitions?
		Status = newStatus;
	}

	public void AddItem(Product product)
	{
		var orderItemToAdd = OrderItem.Create(Guid.NewGuid(), Id, product.Id, product.Price);

		_orderItems.Add(orderItemToAdd);
	}
}
