using SharedKernel.Interfaces;

namespace OrderService.Domain.Events;

public sealed record OrderPlaced : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public Guid AggregateId { get; }
    public long AggregateVersion { get; set; }
    public Guid CustomerId { get; }
    public DateTime PlacedAtUtc { get; }

    public OrderPlaced(Guid orderId, Guid customerId, DateTime placedAtUtc)
    {
        AggregateId = orderId;
        CustomerId = customerId;
        PlacedAtUtc = placedAtUtc;
    }
}