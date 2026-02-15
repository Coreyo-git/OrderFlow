namespace OrderService.Domain.ValueObjects;

/// <summary>
/// Represents the unique identifier for a product.
/// </summary>
public sealed record ProductId
{
	public Guid Value { get; }

	private ProductId(Guid value)
	{
		Value = value;
	}

	/// <summary>
	/// Creates a new, unique <see cref="ProductId"/>.
	/// </summary>
	/// <returns>A new <see cref="ProductId"/> instance.</returns>
	public static ProductId Create() => new(Guid.NewGuid());

	/// <summary>
	/// Creates a <see cref="ProductId"/> from a <see cref="Guid"/>.
	/// </summary>
	/// <param name="value">The GUID value.</param>
	/// <returns>A <see cref="ProductId"/> instance.</returns>
	/// <exception cref="ArgumentException">Thrown when the value is an empty GUID.</exception>
	public static ProductId From(Guid value)
	{
		if (value == Guid.Empty)
			throw new ArgumentException("ProductId cannot be empty.", nameof(value));

		return new ProductId(value);
	}

	/// <summary>
	/// Returns the string representation of the product identifier.
	/// </summary>
	/// <returns>The product ID as a string.</returns>
	public override string ToString() => Value.ToString();
}

