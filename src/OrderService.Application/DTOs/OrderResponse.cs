namespace OrderService.Application.DTOs;

public record OrderItemResponse(
    Guid ProductId,
    decimal Price,
    string Currency
);

public record OrderResponse(
    Guid Id,
    Guid CustomerId,
    string Status,
    IReadOnlyList<OrderItemResponse> Items
);