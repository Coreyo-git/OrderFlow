using SharedKernel.Interfaces;

namespace SharedKernel;

/// <summary>
/// Base class for aggregate roots that need to raise domain events as a
/// side effect of their behaviour, without the aggregate itself knowing
/// who (if anyone) is listening.
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}