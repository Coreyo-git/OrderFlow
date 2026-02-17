using FluentAssertions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Should_create_money_with_valid_data()
    {
        // Arrange
        var currency = "AUD";
        var quantity = 100.50m;

        // Act
        var money = Money.From(currency, quantity);

        // Assert
        money.Should().NotBeNull();
        money.Currency.Should().Be(currency);
        money.Quantity.Should().Be(quantity);
    }

    [Fact]
    public void Should_throw_argument_exception_for_empty_currency()
    {
        // Act
        Action act = () => Money.From("", 100);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Currency must not be null or empty.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_throw_argument_exception_for_invalid_quantity(decimal quantity)
    {
        // Act
        Action act = () => Money.From("AUD", quantity);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Quantity must be greater than 0");
    }

    [Fact]
    public void Should_trim_currency_string()
    {
        // Arrange
        var currency = "  AUD  ";
        var quantity = 100;

        // Act
        var money = Money.From(currency, quantity);

        // Assert
        money.Currency.Should().Be("AUD");
    }

    [Fact]
    public void Should_return_correct_string_representation()
    {
        // Arrange
        var money = Money.From("AUD", 123.45m);
        var culture = Culture.From("en-AU");

        // Act
        var result = money.ToString(culture);

        // Assert
        result.Should().Contain("123.45");
        result.Should().Contain("AUD");
        result.Should().Contain("$");
    }

    [Fact]
    public void Should_be_equal_when_values_are_the_same()
    {
        // Arrange
        var money1 = Money.From("AUD", 100);
        var money2 = Money.From("AUD", 100);

        // Assert
        money1.Should().Be(money2);
    }

    [Fact]
    public void Should_not_be_equal_when_currency_is_different()
    {
        // Arrange
        var money1 = Money.From("AUD", 100);
        var money2 = Money.From("EUR", 100);

        // Assert
        money1.Should().NotBe(money2);
    }

    [Fact]
    public void Should_not_be_equal_when_quantity_is_different()
    {
        // Arrange
        var money1 = Money.From("AUD", 100);
        var money2 = Money.From("AUD", 200);

        // Assert
        money1.Should().NotBe(money2);
    }
}
