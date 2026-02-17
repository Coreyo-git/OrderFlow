using FluentAssertions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Tests.ValueObjects;

public class SkuTests
{
    [Fact]
    public void Should_create_sku_with_valid_value()
    {
        // Arrange
        var value = "SKU-123-ABC";

        // Act
        var sku = Sku.Create(value);

        // Assert
        sku.Should().NotBeNull();
        sku.Value.Should().Be(value);
    }

    [Fact]
    public void Should_throw_argument_exception_for_empty_value()
    {
        // Act
        Action act = () => Sku.Create("");

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Sku creation value cannot be null or empty.");
    }

    [Fact]
    public void Should_be_equal_when_values_are_the_same()
    {
        // Arrange
        var sku1 = Sku.Create("SKU-123");
        var sku2 = Sku.Create("SKU-123");

        // Assert
        sku1.Should().Be(sku2);
    }

    [Fact]
    public void Should_not_be_equal_when_values_are_different()
    {
        // Arrange
        var sku1 = Sku.Create("SKU-123");
        var sku2 = Sku.Create("SKU-456");

        // Assert
        sku1.Should().NotBe(sku2);
    }

    [Fact]
    public void Should_return_value_for_tostring()
    {
        // Arrange
        var value = "SKU-XYZ";
        var sku = Sku.Create(value);

		// Act
		var result = sku.ToString();
		
		// Assert
        Assert.Equal(value, sku.ToString());
    }
}
