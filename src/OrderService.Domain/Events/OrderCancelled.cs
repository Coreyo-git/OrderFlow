using SharedKernel.Interfaces;

namespace OrderService.Domain.Events;

public sealed record OrderCancelled : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public Guid AggregateId { get; }
    public long AggregateVersion { get; set; }
    public DateTime CancelledAtUtc { get; }

    public OrderCancelled(Guid orderId, DateTime cancelledAtUtc)
    {
        AggregateId = orderId;
        CancelledAtUtc = cancelledAtUtc;
    }
}