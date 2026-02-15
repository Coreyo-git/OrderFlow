namespace OrderService.Domain.ValueObjects;

/// <summary>
/// Represents a unique identifier for an order.
/// </summary>
public sealed record OrderId
{
	public Guid Value { get; }

	private OrderId(Guid value)
	{
		Value = value;
	}

	/// <summary>
	/// Creates a new, unique <see cref="OrderId"/>.
	/// </summary>
	/// <returns>A new <see cref="OrderId"/> instance.</returns>
	public static OrderId Create() => new(Guid.NewGuid());

	/// <summary>
	/// Creates an <see cref="OrderId"/> from a <see cref="Guid"/>.
	/// </summary>
	/// <param name="value">The GUID value.</param>
	/// <returns>An <see cref="OrderId"/> instance.</returns>
	/// <exception cref="ArgumentException">Thrown when the value is an empty GUID.</exception>
	public static OrderId From(Guid value)
	{
		if (value == Guid.Empty)
			throw new ArgumentException("OrderId cannot be empty.", nameof(value));

		return new OrderId(value);
	}

	/// <summary>
	/// Returns the string representation of the order identifier.
	/// </summary>
	/// <returns>The order ID as a string.</returns>
	public override string ToString() => Value.ToString();
}

