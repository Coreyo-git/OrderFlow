using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using OrderService.API.IntegrationTests.Fixtures;
using OrderService.Application.DTOs;
using OrderService.Infrastructure.Persistence;

namespace OrderService.API.IntegrationTests;

[Collection("OrderApi")]
public class OrderEndpointsTests : IAsyncLifetime
{
    private readonly OrderApiFixture _fixture;
    private readonly HttpClient _client;

    public OrderEndpointsTests(OrderApiFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.CreateClient();
    }

    public async Task InitializeAsync()
    {
        using var scope = _fixture.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE orders, order_items RESTART IDENTITY CASCADE");
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private static CreateOrderRequest BuildCreateOrderRequest(Guid? customerId = null) => new(
        customerId ?? Guid.NewGuid(),
        new AddressRequest("123 Main St", "Anytown", "QLD", "4124", "Australia"),
        null,
        "en-AU",
        [new CreateOrderItemRequest(Guid.NewGuid(), "Test Product", "TESTSKU123", 10.99m, "USD")]);

    private async Task<OrderResponse> CreateOrderAsync(Guid? customerId = null)
    {
        var response = await _client.PostAsJsonAsync("/api/orders", BuildCreateOrderRequest(customerId));
        response.EnsureSuccessStatusCode();

        var created = await response.Content.ReadFromJsonAsync<OrderResponse>();
        return created!;
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedOrder()
    {
        var customerId = Guid.NewGuid();
        var request = BuildCreateOrderRequest(customerId);

        var response = await _client.PostAsJsonAsync("/api/orders", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<OrderResponse>();
        body!.CustomerId.Should().Be(customerId);
        body.Status.Should().Be("Placed");
        body.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMatchingOrder()
    {
        var created = await CreateOrderAsync();

        var response = await _client.GetAsync($"/api/orders/{created.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<OrderResponse>();
        body!.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task ConfirmAsync_SetsOrderConfirmed()
    {
        var created = await CreateOrderAsync();

        var response = await _client.PatchAsync($"/api/orders/{created.Id}/confirm", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<OrderResponse>();
        body!.Status.Should().Be("Confirmed");
    }

    [Fact]
    public async Task ConfirmAsync_ReturnsNotFound_ForUnknownOrder()
    {
        var response = await _client.PatchAsync($"/api/orders/{Guid.NewGuid()}/confirm", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CancelAsync_SetsOrderCancelled()
    {
        var created = await CreateOrderAsync();

        var response = await _client.PatchAsync($"/api/orders/{created.Id}/cancel", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<OrderResponse>();
        body!.Status.Should().Be("Cancelled");
    }
}