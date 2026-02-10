using FluentAssertions;
using OrderFlow.SharedKernel.ValueObjects;

namespace CustomerService.Domain.Tests.ValueObjects;

public class AddressTests
{
	private const string ValidStreet = "123 Main St";
	private const string ValidCity = "Anytown";
	private const string ValidState = "CA";
	private const string ValidPostalCode = "90210";
	private const string ValidCountry = "USA";

	public class From
	{
		[Fact]
		public void Should_create_from_valid_parameters()
		{
			// Act
			var address = Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, ValidCountry);

			// Assert
			address.Should().NotBeNull();
			address.Street.Should().Be(ValidStreet);
			address.City.Should().Be(ValidCity);
			address.State.Should().Be(ValidState);
			address.PostalCode.Should().Be(ValidPostalCode);
			address.Country.Should().Be(ValidCountry);
		}

		[Fact]
		public void Should_trim_whitespace_from_parameters()
		{
			// Act
			var address = Address.From("  123 Main St  ", "  Anytown  ", "  CA  ", "  90210  ", "  USA  ");

			// Assert
			address.Street.Should().Be(ValidStreet);
			address.City.Should().Be(ValidCity);
			address.State.Should().Be(ValidState);
			address.PostalCode.Should().Be(ValidPostalCode);
			address.Country.Should().Be(ValidCountry);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Should_throw_on_empty_or_whitespace_street(string? value)
		{
			// Act
			var action = () => Address.From(value!, ValidCity, ValidState, ValidPostalCode, ValidCountry);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*Street cannot be empty.*");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Should_throw_on_empty_or_whitespace_city(string? value)
		{
			// Act
			var action = () => Address.From(ValidStreet, value!, ValidState, ValidPostalCode, ValidCountry);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*City cannot be empty.*");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Should_throw_on_empty_or_whitespace_state(string? value)
		{
			// Act
			var action = () => Address.From(ValidStreet, ValidCity, value!, ValidPostalCode, ValidCountry);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*State cannot be empty.*");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Should_throw_on_empty_or_whitespace_postal_code(string? value)
		{
			// Act
			var action = () => Address.From(ValidStreet, ValidCity, ValidState, value!, ValidCountry);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*Postal code cannot be empty.*");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Should_throw_on_empty_or_whitespace_country(string? value)
		{
			// Act
			var action = () => Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, value!);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*Country cannot be empty.*");
		}
	}

	public class Equality
	{
		[Fact]
		public void Should_be_equal_when_all_values_match()
		{
			// Arrange
			var address1 = Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, ValidCountry);
			var address2 = Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, ValidCountry);

			// Assert
			address1.Should().Be(address2);
		}

		[Fact]
		public void Should_not_be_equal_when_street_differs()
		{
			// Arrange
			var address1 = Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, ValidCountry);
			var address2 = Address.From("456 Other Rd", ValidCity, ValidState, ValidPostalCode, ValidCountry);

			// Assert
			address1.Should().NotBe(address2);
		}

		[Fact]
		public void Should_not_be_equal_when_city_differs()
		{
			// Arrange
			var address1 = Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, ValidCountry);
			var address2 = Address.From(ValidStreet, "Othertown", ValidState, ValidPostalCode, ValidCountry);

			// Assert
			address1.Should().NotBe(address2);
		}

		[Fact]
		public void Should_not_be_equal_when_state_differs()
		{
			// Arrange
			var address1 = Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, ValidCountry);
			var address2 = Address.From(ValidStreet, ValidCity, "NY", ValidPostalCode, ValidCountry);

			// Assert
			address1.Should().NotBe(address2);
		}

		[Fact]
		public void Should_not_be_equal_when_postal_code_differs()
		{
			// Arrange
			var address1 = Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, ValidCountry);
			var address2 = Address.From(ValidStreet, ValidCity, ValidState, "99999", ValidCountry);

			// Assert
			address1.Should().NotBe(address2);
		}

		[Fact]
		public void Should_not_be_equal_when_country_differs()
		{
			// Arrange
			var address1 = Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, ValidCountry);
			var address2 = Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, "Canada");

			// Assert
			address1.Should().NotBe(address2);
		}
	}

	public class ToStringMethod
	{
		[Fact]
		public void Should_return_formatted_address_string()
		{
			// Arrange
			var address = Address.From(ValidStreet, ValidCity, ValidState, ValidPostalCode, ValidCountry);
			var expectedString = $"{ValidStreet}, {ValidCity}, {ValidState} {ValidPostalCode}, {ValidCountry}";

			// Act & Assert
			address.ToString().Should().Be(expectedString);
		}
	}
}
