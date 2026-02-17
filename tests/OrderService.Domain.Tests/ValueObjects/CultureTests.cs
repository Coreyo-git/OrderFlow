using FluentAssertions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Tests.ValueObjects;

public class CultureTests
{
    [Fact]
    public void Should_create_culture_with_valid_value()
    {
        // Arrange
        var value = "en-AU";

        // Act
        var culture = Culture.From(value);

        // Assert
        culture.Should().NotBeNull();
        culture.Value.Should().Be(value);
    }

    [Fact]
    public void Should_throw_argument_exception_for_empty_value()
    {
        // Act
        Action act = () => Culture.From("");

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Culture cannot be null or empty.");
    }
}
