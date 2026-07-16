using Microsoft.Extensions.Logging;

using OrderService.Domain.Events;

using SharedKernel.Interfaces;

namespace OrderService.Application.EventHandlers;

public sealed class OrderCancelledHandler : IDomainEventHandler<OrderCancelled>
{
    private readonly ILogger<OrderCancelledHandler> _logger;

    public OrderCancelledHandler(ILogger<OrderCancelledHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderCancelled domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Order {OrderId} cancelled at {CancelledAtUtc}",
            domainEvent.AggregateId,
            domainEvent.CancelledAtUtc);

        return Task.CompletedTask;
    }
}