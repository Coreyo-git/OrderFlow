namespace OrderService.Domain.ValueObjects;

public sealed record OrderId
{
	public Guid Value { get; }

	private OrderId(Guid value)
	{
		Value = value;
	}

	public static OrderId Create() => new(Guid.NewGuid());

	public static OrderId From(Guid value)
	{
		if (value == Guid.Empty)
			throw new ArgumentException("OrderId cannot be empty.", nameof(value));

		return new OrderId(value);
	}

	public override string ToString() => Value.ToString();
}

