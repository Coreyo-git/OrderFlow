using OrderService.Domain.Enums;
using OrderService.Domain.ValueObjects;
using OrderFlow.SharedKernel.ValueObjects;
using SharedKernel.ValueObjects;

namespace OrderService.Domain.Aggregates;

public sealed class Order
{
	public OrderId Id { get; private set; }
	public OrderStatus Status { get; private set; }
	public CustomerId CustomerId { get; private set; }

	private Order(CustomerId customerId)
	{
		Id = OrderId.Create();
		Status = OrderStatus.Draft;
		CustomerId = customerId;
	}

	public static Order Create(Guid customerId)
	{
		return new Order(CustomerId.From(customerId));
	}

	public void UpdateOrderStatus(OrderStatus newStatus)
	{
		// Add validation for transitions?
		Status = newStatus;
	}
}
