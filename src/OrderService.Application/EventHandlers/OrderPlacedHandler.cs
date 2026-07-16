using Microsoft.Extensions.Logging;

using OrderService.Domain.Events;

using SharedKernel.Interfaces;

namespace OrderService.Application.EventHandlers;

/// <summary>
/// Stands in for the Phase 3 Kafka integration-event publisher: once other
/// bounded contexts exist (e.g. PaymentService), this is where OrderPlaced
/// would be translated into an outbound message instead of just a log line.
/// </summary>
public sealed class OrderPlacedHandler : IDomainEventHandler<OrderPlaced>
{
    private readonly ILogger<OrderPlacedHandler> _logger;

    public OrderPlacedHandler(ILogger<OrderPlacedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderPlaced domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Order {OrderId} placed by customer {CustomerId} at {PlacedAtUtc}",
            domainEvent.AggregateId,
            domainEvent.CustomerId,
            domainEvent.PlacedAtUtc);

        return Task.CompletedTask;
    }
}