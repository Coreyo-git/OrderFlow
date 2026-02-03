using CustomerService.Domain.ValueObjects;
using FluentAssertions;

namespace CustomerService.Domain.Tests.ValueObjects;

public class CustomerNameTests
{
	public class From
	{
		[Fact]
		public void Should_create_from_valid_name()
		{
			// Act
			var name = CustomerName.From("John Doe");

			// Assert
			name.Value.Should().Be("John Doe");
		}

		[Fact]
		public void Should_trim_whitespace()
		{
			// Act
			var name = CustomerName.From("  John Doe  ");

			// Assert
			name.Value.Should().Be("John Doe");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public void Should_throw_on_empty_or_whitespace(string? value)
		{
			// Act
			var action = () => CustomerName.From(value!);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*cannot be empty*");
		}

		[Fact]
		public void Should_throw_when_exceeds_200_characters()
		{
			// Arrange
			var longName = new string('a', 201);

			// Act
			var action = () => CustomerName.From(longName);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*200 characters*");
		}

		[Fact]
		public void Should_allow_exactly_200_characters()
		{
			// Arrange
			var maxName = new string('a', 200);

			// Act
			var name = CustomerName.From(maxName);

			// Assert
			name.Value.Should().HaveLength(200);
		}
	}

	public class Equality
	{
		[Fact]
		public void Should_be_equal_when_values_match()
		{
			// Act
			var name1 = CustomerName.From("John Doe");
			var name2 = CustomerName.From("John Doe");

			// Assert
			name1.Should().Be(name2);
		}
	}

	public class ToStringMethod
	{
		[Fact]
		public void Should_return_value()
		{
			// Arrange
			var name = CustomerName.From("John Doe");

			// Act & Assert
			name.ToString().Should().Be("John Doe");
		}
	}
}
