using OrderService.Application.DTOs;

namespace OrderService.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<OrderResponse?> ConfirmAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrderResponse?> CancelAsync(Guid id, CancellationToken cancellationToken = default);
}