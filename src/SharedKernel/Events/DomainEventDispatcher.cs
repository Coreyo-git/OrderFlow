using SharedKernel.Interfaces;

namespace SharedKernel.Events;

/// <summary>
/// DI-backed dispatcher: for each event, resolves every
/// <see cref="IDomainEventHandler{T}"/> registered for that event's concrete
/// type and invokes them. The event type is only known at runtime, so
/// resolution and invocation go through reflection (this is the plumbing a
/// library like MediatR would otherwise hide). Invoking through the
/// interface's MethodInfo -- rather than `dynamic` -- matters here: `dynamic`
/// binds against the handler's concrete runtime type, which fails if that
/// type isn't publicly accessible (e.g. an internal handler class).
/// </summary>
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlersType = typeof(IEnumerable<>).MakeGenericType(handlerType);
            var handleMethod = handlerType.GetMethod("Handle")!;

            // The default DI container returns an empty (not null) enumerable when nothing is registered.
            var handlers = (IEnumerable<object>)_serviceProvider.GetService(handlersType)!;

            foreach (var handler in handlers)
            {
                var task = (Task)handleMethod.Invoke(handler, [domainEvent, cancellationToken])!;
                await task;
            }
        }
    }
}