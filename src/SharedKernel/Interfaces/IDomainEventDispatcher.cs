namespace SharedKernel.Interfaces;

/// <summary>
/// Resolves and invokes every registered <see cref="IDomainEventHandler{T}"/>
/// for each raised event. Call this only after the aggregate's changes have
/// been committed, so handlers never react to a change that got rolled back.
/// </summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}