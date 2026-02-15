using OrderService.Domain.Exceptions;

namespace OrderService.Domain.ValueObjects;

/// <summary>
/// Represents a Stock Keeping Unit (SKU).
/// </summary>
public sealed record Sku
{
	private Sku(string value) => Value = value;
	public string Value { get; }

	/// <summary>
	/// Creates a new <see cref="Sku"/> instance.
	/// </summary>
	/// <param name="value">The SKU value.</param>
	/// <returns>A new <see cref="Sku"/> instance.</returns>
	/// <exception cref="ArgumentException">Thrown when the value is null or empty.</exception>
	public static Sku Create(string value)
	{
		if (string.IsNullOrEmpty(value))
			throw new ArgumentException("Sku creation value cannot be null or empty.");
			
		return new Sku(value);
	}
}