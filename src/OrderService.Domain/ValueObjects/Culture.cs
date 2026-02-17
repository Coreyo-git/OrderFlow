namespace OrderService.Domain.ValueObjects;

public sealed record Culture
{
    public string Value { get; }

    private Culture(string value)
    {
        Value = value;
    }

    public static Culture From(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Culture cannot be null or empty.");
        }

        return new Culture(value);
    }
}
