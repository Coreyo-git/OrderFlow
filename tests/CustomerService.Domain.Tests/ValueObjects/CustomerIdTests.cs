using FluentAssertions;
using SharedKernel.ValueObjects;

namespace CustomerService.Domain.Tests.ValueObjects;

public class CustomerIdTests
{
	public class Create
	{
		[Fact]
		public void Should_return_non_empty_guid()
		{
			// Act
			var id = CustomerId.Create();

			// Assert
			id.Value.Should().NotBe(Guid.Empty);
		}

		[Fact]
		public void Should_return_unique_ids()
		{
			// Act
			var id1 = CustomerId.Create();
			var id2 = CustomerId.Create();

			// Assert
			id1.Value.Should().NotBe(id2.Value);
		}
	}

	public class From
	{
		[Fact]
		public void Should_create_from_valid_guid()
		{
			// Arrange
			var guid = Guid.NewGuid();

			// Act
			var id = CustomerId.From(guid);

			// Assert
			id.Value.Should().Be(guid);
		}

		[Fact]
		public void Should_throw_on_empty_guid()
		{
			// Act
			var action = () => CustomerId.From(Guid.Empty);

			// Assert
			action.Should().Throw<ArgumentException>()
				.WithMessage("*cannot be empty*");
		}
	}

	public class Equality
	{
		[Fact]
		public void Should_be_equal_when_values_match()
		{
			// Arrange
			var guid = Guid.NewGuid();

			// Act
			var id1 = CustomerId.From(guid);
			var id2 = CustomerId.From(guid);

			// Assert
			id1.Should().Be(id2);
		}
	}
}
