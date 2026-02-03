using FluentAssertions;
using SharedKernel.ValueObjects;

namespace CustomerService.Domain.Tests.ValueObjects;

public class EmailTests
{
	public class From
	{
		[Theory]
		[InlineData("test@example.com")]
		[InlineData("user.name@domain.co.uk")]
		[InlineData("user+tag@example.org")]
		public void Should_create_from_valid_email(string value)
		{
			// Act
			var email = Email.From(value);

			// Assert
			email.Value.Should().Be(value.ToLowerInvariant());
		}

		[Fact]
		public void Should_normalize_to_lowercase()
		{
			// Act
			var email = Email.From("John.Doe@Example.COM");

			// Assert
			email.Value.Should().Be("john.doe@example.com");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Should_throw_on_empty_or_whitespace(string? value)
		{
			// Act
			var action = () => Email.From(value!);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*cannot be empty*");
		}

		[Theory]
		[InlineData("notanemail")]
		[InlineData("missing@domain")]
		[InlineData("@nodomain.com")]
		[InlineData("no@domain.")]
		[InlineData("spaces in@email.com")]
		public void Should_throw_on_invalid_format(string value)
		{
			// Act
			var action = () => Email.From(value);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*invalid*");
		}
	}

	public class Equality
	{
		[Fact]
		public void Should_be_equal_when_values_match()
		{
			// Act
			var email1 = Email.From("test@example.com");
			var email2 = Email.From("test@example.com");

			// Assert
			email1.Should().Be(email2);
		}

		[Fact]
		public void Should_be_equal_regardless_of_case()
		{
			// Act
			var email1 = Email.From("Test@Example.com");
			var email2 = Email.From("test@example.com");

			// Assert
			email1.Should().Be(email2);
		}
	}

	public class ToStringMethod
	{
		[Fact]
		public void Should_return_value()
		{
			// Arrange
			var email = Email.From("test@example.com");

			// Act & Assert
			email.ToString().Should().Be("test@example.com");
		}
	}
}
