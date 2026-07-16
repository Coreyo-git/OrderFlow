using Microsoft.Extensions.Logging;

using OrderService.Domain.Events;

using SharedKernel.Interfaces;

namespace OrderService.Application.EventHandlers;

public sealed class OrderConfirmedHandler : IDomainEventHandler<OrderConfirmed>
{
    private readonly ILogger<OrderConfirmedHandler> _logger;

    public OrderConfirmedHandler(ILogger<OrderConfirmedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderConfirmed domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Order {OrderId} confirmed at {ConfirmedAtUtc}",
            domainEvent.AggregateId,
            domainEvent.ConfirmedAtUtc);

        return Task.CompletedTask;
    }
}