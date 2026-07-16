using OrderService.Domain.Aggregates;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default);

    void Add(Order order);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}