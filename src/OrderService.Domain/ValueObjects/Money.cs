using OrderService.Domain.Exceptions;

namespace OrderService.Domain.ValueObjects;

public sealed record Money
{
	public string Currency { get; } = string.Empty;
	public decimal Quantity { get; }

	private Money(string currency, decimal quantity)
	{
		Currency = currency;
		Quantity = quantity;
	}

	public static Money From(string currency, decimal quantity)
	{
		if (string.IsNullOrEmpty(currency))
			throw new ArgumentException("Currency must not be null or empty.");
		if (quantity <= 0)
			throw new ArgumentException("Quantity must be greater than 0");

		string currencyTrimmed = currency.Trim();

		return new Money(currencyTrimmed, quantity);
	}

	public override string ToString() => $"{Quantity.ToString("C")} {Currency}";
}