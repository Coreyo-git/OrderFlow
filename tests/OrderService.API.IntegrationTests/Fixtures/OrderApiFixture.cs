using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using OrderService.Infrastructure.Persistence;

using Testcontainers.PostgreSql;

namespace OrderService.API.IntegrationTests.Fixtures;

public class OrderApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string DefaultPostgresImage = "postgres:16-alpine";

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder(
            Environment.GetEnvironmentVariable("POSTGRES_IMAGE") ?? DefaultPostgresImage)
        .WithDatabase("orderdb")
        .WithUsername("orderflow")
        .WithPassword("orderflow_dev")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<OrderDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<OrderDbContext>(options =>
                options.UseNpgsql(_container.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        await context.Database.MigrateAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}