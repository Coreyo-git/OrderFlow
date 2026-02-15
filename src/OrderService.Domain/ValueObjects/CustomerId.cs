namespace OrderService.Domain.ValueObjects;

/// <summary>
/// Represents the unique identifier for a customer from the CustomerService.
/// </summary>
public sealed record CustomerId
{
	public Guid Value { get; }

	private CustomerId(Guid value)
	{
		Value = value;
	}

	/// <summary>
	/// Creates a <see cref="CustomerId"/> from a <see cref="Guid"/>.
	/// </summary>
	/// <param name="value">The GUID value.</param>
	/// <returns>A <see cref="CustomerId"/> instance.</returns>
	/// <exception cref="ArgumentException">Thrown when the value is an empty GUID.</exception>
	public static CustomerId From(Guid value)
	{
		if (value == Guid.Empty)
			throw new ArgumentException("CustomerId cannot be empty.", nameof(value));

		return new CustomerId(value);
	}

	/// <summary>
	/// Returns the string representation of the product identifier.
	/// </summary>
	/// <returns>The product ID as a string.</returns>
	public override string ToString() => Value.ToString();
}

