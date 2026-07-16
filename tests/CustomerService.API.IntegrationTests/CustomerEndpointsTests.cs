using System.Net;
using System.Net.Http.Json;

using CustomerService.API.IntegrationTests.Fixtures;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Events;
using CustomerService.Infrastructure.Persistence;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SharedKernel.Interfaces;

namespace CustomerService.API.IntegrationTests;

[Collection("CustomerApi")]
public class CustomerEndpointsTests : IAsyncLifetime
{
    private readonly CustomerApiFixture _fixture;
    private readonly HttpClient _client;

    public CustomerEndpointsTests(CustomerApiFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.CreateClient();
    }

    public async Task InitializeAsync()
    {
        using var scope = _fixture.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
        await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE customers RESTART IDENTITY CASCADE");
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private async Task<CustomerResponse> CreateCustomerAsync(string email)
    {
        var request = new CreateCustomerRequest("Ada Lovelace", email, null, null);

        var response = await _client.PostAsJsonAsync("/api/customers", request);
        response.EnsureSuccessStatusCode();

        var created = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        return created!;
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedCustomer()
    {
        var request = new CreateCustomerRequest("Ada Lovelace", "ada@example.com", null, null);

        var response = await _client.PostAsJsonAsync("/api/customers", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        body!.Email.Should().Be("ada@example.com");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMatchingCustomer()
    {
        var created = await CreateCustomerAsync("grace@example.com");

        var response = await _client.GetAsync($"/api/customers/{created.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        body!.Email.Should().Be("grace@example.com");
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsMatchingCustomer()
    {
        var created = await CreateCustomerAsync("katherine@example.com");

        var response = await _client.GetAsync("/api/customers/by-email?email=katherine@example.com");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        body!.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task GetAllAsync_IncludesCreatedCustomer()
    {
        var created = await CreateCustomerAsync("margaret@example.com");

        var response = await _client.GetAsync("/api/customers");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<List<CustomerResponse>>();
        body!.Should().ContainSingle(c => c.Id == created.Id);
    }

    [Fact]
    public async Task DeactivateAsync_SetsCustomerInactive()
    {
        var created = await CreateCustomerAsync("turing@example.com");

        var response = await _client.PatchAsync($"/api/customers/{created.Id}/deactivate", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        body!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsNotFound_ForUnknownCustomer()
    {
        var response = await _client.PatchAsync($"/api/customers/{Guid.NewGuid()}/deactivate", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeactivateAsync_DispatchesCustomerDeactivatedToRegisteredHandlers()
    {
        // Proves the dispatcher's DI resolution + fan-out actually works end to end,
        // through the real HTTP pipeline rather than a bare unit test of the dispatcher.
        var spy = new SpyCustomerDeactivatedHandler();

        using var factory = _fixture.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IDomainEventHandler<CustomerDeactivated>>(spy);
            });
        });
        using var client = factory.CreateClient();

        var createResponse = await client.PostAsJsonAsync(
            "/api/customers",
            new CreateCustomerRequest("Alan Turing", "turing-spy@example.com", null, null));
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created, await createResponse.Content.ReadAsStringAsync());
        var created = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        var patchResponse = await client.PatchAsync($"/api/customers/{created!.Id}/deactivate", null);
        patchResponse.StatusCode.Should().Be(HttpStatusCode.OK, await patchResponse.Content.ReadAsStringAsync());

        spy.HandledEvents.Should().ContainSingle(e => e.AggregateId == created.Id);
    }

    private sealed class SpyCustomerDeactivatedHandler : IDomainEventHandler<CustomerDeactivated>
    {
        public List<CustomerDeactivated> HandledEvents { get; } = new();

        public Task Handle(CustomerDeactivated domainEvent, CancellationToken cancellationToken = default)
        {
            HandledEvents.Add(domainEvent);
            return Task.CompletedTask;
        }
    }
}