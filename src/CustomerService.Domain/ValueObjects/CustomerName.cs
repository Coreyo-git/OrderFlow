namespace CustomerService.Domain.ValueObjects;

public sealed record CustomerName
{
	public string Value { get; }
	private CustomerName(string value) => Value = value;

	public static CustomerName From(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentException("Customer name cannot be empty.", nameof(value));

		if (value.Length > 200)
			throw new ArgumentException("Customer name cannot exceed 200 characters.", nameof(value));

		var trimmed = value.Trim();

		return new CustomerName(trimmed);
	}

	public override string ToString() => Value;
}