using CustomerService.Domain.ValueObjects;
using FluentAssertions;

namespace CustomerService.Domain.Tests.ValueObjects;

public class PhoneNumberTests
{
	public class From
	{
		[Fact]
		public void Should_create_from_valid_number()
		{
			// Act
			var phone = PhoneNumber.From("12345678");

			// Assert
			phone.Value.Should().Be("12345678");
		}

		[Theory]
		[InlineData("1234-5678", "12345678")]
		[InlineData("(02) 1234 5678", "0212345678")]
		[InlineData("+61 412 345 678", "61412345678")]
		[InlineData("123.456.7890", "1234567890")]
		public void Should_strip_non_digit_characters(string input, string expected)
		{
			// Act
			var phone = PhoneNumber.From(input);

			// Assert
			phone.Value.Should().Be(expected);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Should_throw_on_empty_or_whitespace(string? value)
		{
			// Act
			var action = () => PhoneNumber.From(value!);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*cannot be empty*");
		}

		[Fact]
		public void Should_throw_when_less_than_8_digits()
		{
			// Act
			var action = () => PhoneNumber.From("1234567"); // 7 digits

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*8 and 15 digits*");
		}

		[Fact]
		public void Should_throw_when_more_than_15_digits()
		{
			// Arrange
			var tooLong = new string('1', 16);

			// Act
			var action = () => PhoneNumber.From(tooLong);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*8 and 15 digits*");
		}

		[Fact]
		public void Should_allow_exactly_8_digits()
		{
			// Act
			var phone = PhoneNumber.From("12345678");

			// Assert
			phone.Value.Should().HaveLength(8);
		}

		[Fact]
		public void Should_allow_exactly_15_digits()
		{
			// Arrange
			var maxDigits = new string('1', 15);

			// Act
			var phone = PhoneNumber.From(maxDigits);

			// Assert
			phone.Value.Should().HaveLength(15);
		}
	}

	public class FromNullable
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Should_return_null_for_null_or_empty(string? value)
		{
			// Act
			var phone = PhoneNumber.FromNullable(value);

			// Assert
			phone.Should().BeNull();
		}

		[Fact]
		public void Should_return_phone_number_for_valid_input()
		{
			// Act
			var phone = PhoneNumber.FromNullable("12345678");

			// Assert
			phone.Should().NotBeNull();
			phone!.Value.Should().Be("12345678");
		}
	}

	public class Equality
	{
		[Fact]
		public void Should_be_equal_when_values_match()
		{
			// Act
			var phone1 = PhoneNumber.From("12345678");
			var phone2 = PhoneNumber.From("12345678");

			// Assert
			phone1.Should().Be(phone2);
		}

		[Fact]
		public void Should_be_equal_when_normalized_values_match()
		{
			// Act
			var phone1 = PhoneNumber.From("1234-5678");
			var phone2 = PhoneNumber.From("12345678");

			// Assert
			phone1.Should().Be(phone2);
		}
	}

	public class ToStringMethod
	{
		[Fact]
		public void Should_return_value()
		{
			// Arrange
			var phone = PhoneNumber.From("12345678");

			// Act & Assert
			phone.ToString().Should().Be("12345678");
		}
	}
}
