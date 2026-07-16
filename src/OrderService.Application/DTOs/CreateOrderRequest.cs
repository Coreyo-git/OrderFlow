namespace OrderService.Application.DTOs;

public record CreateOrderItemRequest(
    Guid ProductId,
    string ProductName,
    string Sku,
    decimal Price,
    string Currency
);

public record CreateOrderRequest(
    Guid CustomerId,
    AddressRequest ShippingAddress,
    AddressRequest? BillingAddress,
    string Culture,
    List<CreateOrderItemRequest> Items
);