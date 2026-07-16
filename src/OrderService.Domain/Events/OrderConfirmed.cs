using SharedKernel.Interfaces;

namespace OrderService.Domain.Events;

public sealed record OrderConfirmed : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public Guid AggregateId { get; }
    public long AggregateVersion { get; set; }
    public DateTime ConfirmedAtUtc { get; }

    public OrderConfirmed(Guid orderId, DateTime confirmedAtUtc)
    {
        AggregateId = orderId;
        ConfirmedAtUtc = confirmedAtUtc;
    }
}