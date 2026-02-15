namespace CustomerService.Domain.ValueObjects;

/// <summary>
/// Represents a customer's name.
/// </summary>
public sealed record CustomerName
{
	public string Value { get; }
	private CustomerName(string value) => Value = value;

	/// <summary>
	/// Creates a new <see cref="CustomerName"/> instance from the specified string.
	/// </summary>
	/// <param name="value">The name string.</param>
	/// <returns>A new <see cref="CustomerName"/> instance.</returns>
	/// <exception cref="ArgumentException">Thrown when the value is empty or exceeds 200 characters.</exception>
	public static CustomerName From(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentException("Customer name cannot be empty.", nameof(value));

		if (value.Length > 200)
			throw new ArgumentException("Customer name cannot exceed 200 characters.", nameof(value));

		var trimmed = value.Trim();

		return new CustomerName(trimmed);
	}

	/// <summary>
	/// Returns the string representation of the customer name.
	/// </summary>
	/// <returns>The customer name string.</returns>
	public override string ToString() => Value;
}