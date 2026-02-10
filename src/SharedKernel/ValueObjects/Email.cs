using System.Text.RegularExpressions;

namespace OrderFlow.SharedKernel.ValueObjects;

public sealed partial record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.", nameof(value));

        if (!EmailRegex().IsMatch(value))
            throw new ArgumentException("Email format is invalid.", nameof(value));

        return new Email(value.ToLowerInvariant());
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();
}
