namespace OrderService.Domain.Enums;

public enum OrderStatus
{
	/// <summary>
	/// The user has submitted the order, and it is awaiting processing by the Payment Service.
	/// </summary>
	Placed,
	/// <summary>
	/// Payment has been successfully processed, and the order is awaiting fulfillment/shipping confirmation.
	/// </summary>
	Confirmed,
	/// <summary>
	/// The order has been shipped and is in transit, awaiting final completion.
	/// </summary>
	Shipped,
	/// <summary>
	/// The order has been successfully delivered and completed.
	/// </summary>
	Completed,
	/// <summary>
	/// The order has been cancelled and will not be fulfilled.
	/// </summary>
	Cancelled
}