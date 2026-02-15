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
	public Address ShippingAddress { get; private set; }
	public Address? BillingAddress { get; private set; }

	/// <summary>
	/// Private constructor for creating a new order.
	/// Sets the initial status to Placed and generates a new OrderId.
	/// </summary>
	private Order(CustomerId customerId, Address shippingAddress, Address? billingAddress)
	{
		Id = OrderId.Create();
		Status = OrderStatus.Placed; // Orders are now created directly in the Placed state.
		CustomerId = customerId;
		ShippingAddress = shippingAddress;
		BillingAddress = billingAddress;
	}

	/// <summary>
	/// Creates a new order instance atomically with all its items.
	/// </summary>
	/// <param name="customerId">The customer identifier.</param>
	/// <param name="shippingAddress">The customer's shipping address.</param>
	/// <param name="billingAddress">The customer's billing address (optional).</param>
	/// <param name="products">A collection of products to be included in the order.</param>
	/// <returns>A new <see cref="Order"/> instance initialized in the Placed status.</returns>
	/// <exception cref="DomainException">Thrown if the product list is null or empty.</exception>
	public static Order Create(CustomerId customerId, Address shippingAddress, Address? billingAddress, IReadOnlyCollection<Product> products)
	{
		if (products is not { Count: > 0 })
		{
			throw new DomainException("An order must contain at least one item.");
		}
		
		var order = new Order(customerId, shippingAddress, billingAddress);

		foreach (var product in products)
		{
			var orderItem = OrderItem.Create(Guid.NewGuid(), order.Id, product.Id, product.Price);
			order._orderItems.Add(orderItem);
		}

		return order;
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
