using CustomerService.Domain.Events;

using Microsoft.Extensions.Logging;

using SharedKernel.Interfaces;

namespace CustomerService.Application.EventHandlers;

/// <summary>
/// Stands in for the Phase 3 Kafka integration-event publisher: once other
/// bounded contexts exist, this is where CustomerDeactivated would be
/// translated into an outbound message instead of just a log line.
/// </summary>
public sealed class CustomerDeactivatedHandler : IDomainEventHandler<CustomerDeactivated>
{
    private readonly ILogger<CustomerDeactivatedHandler> _logger;

    public CustomerDeactivatedHandler(ILogger<CustomerDeactivatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerDeactivated domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Customer {CustomerId} deactivated at {DeactivatedAtUtc}",
            domainEvent.AggregateId,
            domainEvent.DeactivatedAtUtc);

        return Task.CompletedTask;
    }
}