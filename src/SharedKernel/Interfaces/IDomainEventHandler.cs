namespace SharedKernel.Interfaces;

// Generic interface for handling domain events.
public interface IDomainEventHandler<in T> where T : IDomainEvent
{
	Task Handle(T domainEvent, CancellationToken cancellationToken = default);
}
