using System.Globalization;
using OrderService.Domain.Exceptions;

namespace OrderService.Domain.ValueObjects;

/// <summary>
/// Represents a monetary value.
/// </summary>
public sealed record Money
{
	public string Currency { get; } = string.Empty;
	public decimal Quantity { get; }

	private Money(string currency, decimal quantity)
	{
		Currency = currency;
		Quantity = quantity;
	}

	/// <summary>
	/// Creates a new <see cref="Money"/> instance.
	/// </summary>
	/// <param name="currency">The currency code.</param>
	/// <param name="quantity">The monetary amount.</param>
	/// <returns>A new <see cref="Money"/> instance.</returns>
	/// <exception cref="ArgumentException">Thrown when currency is null/empty or quantity is not greater than 0.</exception>
	public static Money From(string currency, decimal quantity)
	{
		if (string.IsNullOrEmpty(currency))
			throw new ArgumentException("Currency must not be null or empty.");
		if (quantity <= 0)
			throw new ArgumentException("Quantity must be greater than 0");

		string currencyTrimmed = currency.Trim();

		return new Money(currencyTrimmed, quantity);
	}

	/// <summary>
	/// Returns the string representation of the monetary value.
	/// </summary>
	/// <returns>The formatted monetary value.</returns>
	public override string ToString() => $"{Quantity.ToString("C")} {Currency}";

	/// <summary>
	/// Returns the string representation of the monetary value.
	/// </summary>
	/// <param name="culture">The culture to use for formatting.</param>
	/// <returns>The formatted monetary value.</returns>
	public string ToString(Culture culture)
	{
		var cultureInfo = new CultureInfo(culture.Value);
		return $"{Quantity.ToString("C", cultureInfo)} {Currency}";
	}
}