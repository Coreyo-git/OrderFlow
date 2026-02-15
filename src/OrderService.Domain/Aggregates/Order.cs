using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Aggregates;

/// <summary>
/// Represents an order in the system.
/// </summary>
public sealed class Order
{
	private List<OrderItem> _orderItems = new();
	/// <summary>
	/// Gets the unique identifier for the order.
	/// </summary>
	public OrderId Id { get; private set; }
	/// <summary>
	/// Gets the identifier of the customer who placed the order.
	/// </summary>
	public CustomerId CustomerId { get; private set; }
	/// <summary>
	/// Gets the current status of the order.
	/// </summary>
	public OrderStatus Status { get; private set; }
	/// <summary>
	/// Gets a read-only collection of items included in the order.
	/// </summary>
	public IReadOnlyCollection<OrderItem> OrderItems => _orderItems; 

	/// <summary>
	/// Sets the initial status to Draft and generates a new OrderId.
	/// </summary>
	/// <param name="customerId">The identifier of the customer.</param>
	private Order(CustomerId customerId)
	{
		Id = OrderId.Create();
		Status = OrderStatus.Draft;
		CustomerId = customerId;
	}

	/// <summary>
	/// Creates a new order instance.
	/// </summary>
	/// <param name="customerId">The customer identifier.</param>
	/// <returns>A new <see cref="Order"/> instance initialized in the Draft status.</returns>
	public static Order Create(CustomerId customerId)
	{
		return new Order(customerId);
	}

	/// <summary>
	/// Adds a new item to the order.
	/// Items can only be added when the order is in the Draft status.
	/// </summary>
	/// <param name="product">The product to add.</param>
	/// <exception cref="DomainException">Thrown if items are added when the order is not in Draft status.</exception>
	public void AddItem(Product product)
	{
		if (Status != OrderStatus.Draft)
		{
			throw new DomainException($"Cannot add an item to an order in {Status} state.");
		}

		var orderItemToAdd = OrderItem.Create(Guid.NewGuid(), Id, product.Id, product.Price);

		_orderItems.Add(orderItemToAdd);
	}

	/// <summary>
	/// Places the order, transitioning it from Draft to Placed status.
	/// An order can only be placed if it is currently in Draft status and contains items.
	/// </summary>
	/// <exception cref="DomainException">Thrown if the order is not in Draft status or has no items.</exception>
	public void PlaceOrder()
	{
		if (Status != OrderStatus.Draft)
		{
			throw new DomainException("Only a draft order can be placed.");
		}
		if (_orderItems.Count <= 0)
		{
			throw new DomainException("Order can not be placed without items.");
		}

		Status = OrderStatus.Placed;
	}

	/// <summary>
	/// Confirms the order, typically after successful payment processing,
	/// transitioning it from Placed to Confirmed status.
	/// An order can only be confirmed if it is currently in Placed status.
	/// </summary>
	/// <exception cref="DomainException">Thrown if the order is not in Placed status.</exception>
	public void ConfirmOrder()
	{
		if (Status != OrderStatus.Placed)
		{
			throw new DomainException("Only a placed order can be confirmed.");
		}

		Status = OrderStatus.Confirmed;
	}
	
	/// <summary>
	/// Ships the order, transitioning it from Confirmed to Shipped status.
	/// An order can only be shipped if it is currently in Confirmed status.
	/// </summary>
	/// <exception cref="DomainException">Thrown if the order is not in Confirmed status.</exception>
	public void ShipOrder()
	{
		if (Status != OrderStatus.Confirmed)
		{
			throw new DomainException("Only a confirmed order can be shipped.");
		}

		Status = OrderStatus.Shipped;
	}

	/// <summary>
	/// Completes the order, transitioning it from Shipped to Completed status.
	/// An order can only be completed if it is currently in Shipped status.
	/// </summary>
	/// <exception cref="DomainException">Thrown if the order is not in Shipped status.</exception>
	public void CompleteOrder()
	{
		if (Status != OrderStatus.Shipped)
		{
			throw new DomainException("Only a shipped order can be completed");
		}

		Status = OrderStatus.Completed;
	}
	
	/// <summary>
	/// Cancels the order.
	/// An order cannot be cancelled if it has already been Shipped or Completed.
	/// </summary>
	/// <exception cref="DomainException">Thrown if the order is already Shipped or Completed.</exception>
	public void CancelOrder()
	{
		// An order that is already shipped or completed cannot be cancelled.
		// TODO: Potentially allow shipped orders to be cancelled in case of shipping failure?, revisit
		if (Status == OrderStatus.Shipped || Status == OrderStatus.Completed)
		{
			throw new DomainException($"Cannot cancel an order that is already {Status}.");
		}
		
		// If the order is already cancelled, no action is needed.
		if (Status == OrderStatus.Cancelled)
		{
			return; 
		}

		Status = OrderStatus.Cancelled;
	}
}
