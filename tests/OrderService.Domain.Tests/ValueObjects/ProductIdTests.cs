using FluentAssertions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Tests.ValueObjects;

public class ProductIdTests
{
    [Fact]
    public void Should_create_new_product_id()
    {
        // Act
        var productId1 = ProductId.Create();
        var productId2 = ProductId.Create();

        // Assert
        productId1.Should().NotBeNull();
        productId1.Value.Should().NotBe(Guid.Empty);
        productId1.Should().NotBe(productId2);
    }

    [Fact]
    public void Should_create_product_id_from_valid_guid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var productId = ProductId.From(guid);

        // Assert
        productId.Should().NotBeNull();
        productId.Value.Should().Be(guid);
    }

    [Fact]
    public void Should_throw_argument_exception_for_empty_guid()
    {
        // Act
        Action act = () => ProductId.From(Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("ProductId cannot be empty. (Parameter 'value')");
    }

    [Fact]
    public void Should_return_correct_string_representation()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var productId = ProductId.From(guid);

        // Act
        var result = productId.ToString();

        // Assert
        result.Should().Be(guid.ToString());
    }

    [Fact]
    public void Should_be_equal_when_guids_are_the_same()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var productId1 = ProductId.From(guid);
        var productId2 = ProductId.From(guid);

        // Assert
        productId1.Should().Be(productId2);
    }

    [Fact]
    public void Should_not_be_equal_when_guids_are_different()
    {
        // Arrange
        var productId1 = ProductId.Create();
        var productId2 = ProductId.Create();

        // Assert
        productId1.Should().NotBe(productId2);
    }
}
