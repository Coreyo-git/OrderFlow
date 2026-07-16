using SharedKernel.Interfaces;

namespace CustomerService.Domain.Events;

public sealed record CustomerDeactivated : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public Guid AggregateId { get; }
    public long AggregateVersion { get; set; }
    public DateTime DeactivatedAtUtc { get; }

    public CustomerDeactivated(Guid customerId, DateTime deactivatedAtUtc)
    {
        AggregateId = customerId;
        DeactivatedAtUtc = deactivatedAtUtc;
    }
}
