using OrderFlow.SharedKernel.ValueObjects;

namespace CustomerService.Domain.ValueObjects;

public sealed partial record Address
{
	public AddressId Id { get; }
	public string Street { get; }
	public string City { get; }
	public string State { get; }
	public string PostalCode { get; }
	public string Country { get; }

	private Address(AddressId id, string street, string city, string state, string postalCode, string country)
	{
		Id = id;
		Street = street;
		City = city;
		State = state;
		PostalCode = postalCode;
		Country = country;
	}

	public static Address From(string street, string city, string state, string postalCode, string country)
	{
		if (string.IsNullOrWhiteSpace(street))
			throw new ArgumentException("Street cannot be empty.", nameof(street));
		if (string.IsNullOrWhiteSpace(city))
			throw new ArgumentException("City cannot be empty.", nameof(city));
		if (string.IsNullOrWhiteSpace(state))
			throw new ArgumentException("State cannot be empty.", nameof(state));
		if (string.IsNullOrWhiteSpace(postalCode))
			throw new ArgumentException("Postal code cannot be empty.", nameof(postalCode));
		if (string.IsNullOrWhiteSpace(country))
			throw new ArgumentException("Country cannot be empty.", nameof(country));

		return new Address(
			AddressId.Create(),
			street.Trim(),
			city.Trim(),
			state?.Trim() ?? string.Empty,
			postalCode.Trim(),
			country.Trim()
		);
	}

	public override string ToString() =>
		$"{Street}, {City}, {State} {PostalCode}, {Country}";
}