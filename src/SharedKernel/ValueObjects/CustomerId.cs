namespace SharedKernel.ValueObjects;

public sealed record CustomerId
{
    public Guid Value { get; }

    private CustomerId(Guid value)
    {
        Value = value;
    }

    public static CustomerId Create() => new(Guid.NewGuid());

    public static CustomerId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty.", nameof(value));

        return new CustomerId(value);
    }

    public override string ToString() => Value.ToString();
}
