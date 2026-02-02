using System.Text.RegularExpressions;

namespace SharedKernel.ValueObjects;

public sealed partial record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number cannot be empty.", nameof(value));

        var digitsOnly = DigitsOnlyRegex().Replace(value, "");

        if (digitsOnly.Length < 8 || digitsOnly.Length > 15)
            throw new ArgumentException("Phone number must be between 8 and 15 digits.", nameof(value));

        return new PhoneNumber(digitsOnly);
    }

    public static PhoneNumber? FromNullable(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : From(value);
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();
}
