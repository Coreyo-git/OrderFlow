namespace OrderFlow.SharedKernel.ValueObjects;

public sealed record AddressId
{
	public Guid Value { get; }

	private AddressId(Guid value)
	{
		Value = value;
	}

	public static AddressId Create() => new(Guid.NewGuid());

	public static AddressId From(Guid value)
	{
		if (value == Guid.Empty)
			throw new ArgumentException("CustomerId cannot be empty.", nameof(value));

		return new AddressId(value);
	}

	public override string ToString() => Value.ToString();
}
