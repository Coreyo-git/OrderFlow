using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Aggregates;
using OrderService.Domain.Interfaces;
using OrderService.Domain.ValueObjects;

using SharedKernel.Interfaces;

namespace OrderService.Application.Services;

public class OrderApplicationService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public OrderApplicationService(IOrderRepository orderRepository, IDomainEventDispatcher domainEventDispatcher)
    {
        _orderRepository = orderRepository;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<OrderResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(OrderId.From(id), cancellationToken);
        return order is null ? null : MapToResponse(order);
    }

    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var products = request.Items
            .Select(item => Product.Create(
                ProductId.From(item.ProductId),
                item.ProductName,
                Money.From(item.Currency, item.Price),
                Sku.Create(item.Sku)))
            .ToList();

        var order = Order.Create(
            CustomerId.From(request.CustomerId),
            MapToAddress(request.ShippingAddress),
            request.BillingAddress is null ? null : MapToAddress(request.BillingAddress),
            Culture.From(request.Culture),
            products);

        _orderRepository.Add(order);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        // Dispatch only after the change is committed, so handlers never react to a rolled-back change.
        await _domainEventDispatcher.DispatchAsync(order.DomainEvents, cancellationToken);
        order.ClearDomainEvents();

        return MapToResponse(order);
    }

    public async Task<OrderResponse?> ConfirmAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(OrderId.From(id), cancellationToken);
        if (order is null)
        {
            return null;
        }

        order.ConfirmOrder();
        await _orderRepository.SaveChangesAsync(cancellationToken);

        await _domainEventDispatcher.DispatchAsync(order.DomainEvents, cancellationToken);
        order.ClearDomainEvents();

        return MapToResponse(order);
    }

    public async Task<OrderResponse?> CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(OrderId.From(id), cancellationToken);
        if (order is null)
        {
            return null;
        }

        order.CancelOrder();
        await _orderRepository.SaveChangesAsync(cancellationToken);

        await _domainEventDispatcher.DispatchAsync(order.DomainEvents, cancellationToken);
        order.ClearDomainEvents();

        return MapToResponse(order);
    }

    private static Address MapToAddress(AddressRequest request) =>
        Address.From(request.Street, request.City, request.State, request.PostalCode, request.Country);

    private static OrderResponse MapToResponse(Order order)
    {
        var items = order.OrderItems
            .Select(item => new OrderItemResponse(item.ProductId.Value, item.Price.Quantity, item.Price.Currency))
            .ToList();

        return new OrderResponse(order.Id.Value, order.CustomerId.Value, order.Status.ToString(), items);
    }
}