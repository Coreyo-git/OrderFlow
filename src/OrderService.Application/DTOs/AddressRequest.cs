namespace OrderService.Application.DTOs;

public record AddressRequest(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country
);