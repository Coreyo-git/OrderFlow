using OrderService.Domain.Exceptions;

namespace OrderService.Domain.ValueObjects;

public sealed record Sku
{
	private Sku(string value) => Value = value;
	public string Value { get; }
	public static Sku Create(string value)
	{
		if (string.IsNullOrEmpty(value))
			throw new ArgumentException("Sku creation value cannot be null or empty.");
			
		return new Sku(value);
	}
}