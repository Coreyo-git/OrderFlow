using System.Net;
using System.Net.Http.Json;

using CustomerService.API.IntegrationTests.Fixtures;
using CustomerService.Application.DTOs;
using CustomerService.Infrastructure.Persistence;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
}