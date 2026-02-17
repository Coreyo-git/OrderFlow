using FluentAssertions;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Tests.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Should_create_address_with_valid_data()
    {
        // Arrange
        var street = "123 Fake St";
        var city = "Melbourne";
        var state = "VIC";
        var postalCode = "3000";
        var country = "Australia";

        // Act
        var address = Address.From(street, city, state, postalCode, country);

        // Assert
        address.Should().NotBeNull();
        address.Street.Should().Be(street);
        address.City.Should().Be(city);
        address.State.Should().Be(state);
        address.PostalCode.Should().Be(postalCode);
        address.Country.Should().Be(country);
    }

    [Theory]
    [InlineData("", "Melbourne", "VIC", "3000", "Australia", "Street cannot be empty.")]
    [InlineData("123 Fake St", "", "VIC", "3000", "Australia", "City cannot be empty.")]
    [InlineData("123 Fake St", "Melbourne", "", "3000", "Australia", "State cannot be empty.")]
    [InlineData("123 Fake St", "Melbourne", "VIC", "", "Australia", "Postal code cannot be empty.")]
    [InlineData("123 Fake St", "Melbourne", "VIC", "3000", "", "Country cannot be empty.")]
    public void Should_throw_argument_exception_for_empty_address_data(
        string street,
        string city,
        string state,
        string postalCode,
        string country,
        string expectedMessage
    )
    {
        // Act
        Action act = () => Address.From(street, city, state, postalCode, country);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(expectedMessage + "*");
    }



    [Fact]
    public void Should_trim_whitespace_from_address_properties()
    {
        // Arrange
        var street = "  123 Fake St  ";
        var city = "  Melbourne  ";
        var state = "  VIC  ";
        var postalCode = "  3000  ";
        var country = "  Australia  ";

        // Act
        var address = Address.From(street, city, state, postalCode, country);

        // Assert
        address.Street.Should().Be("123 Fake St");
        address.City.Should().Be("Melbourne");
        address.State.Should().Be("VIC");
        address.PostalCode.Should().Be("3000");
        address.Country.Should().Be("Australia");
    }

    [Fact]
    public void Should_return_correct_string_representation()
    {
        // Arrange
        var address = Address.From("123 Fake St", "Melbourne", "VIC", "3000", "Australia");
        var expectedString = "123 Fake St, Melbourne, VIC 3000, Australia";

        // Act
        var result = address.ToString();

        // Assert
        result.Should().Be(expectedString);
    }}
