using FluentValidation;

using Microsoft.EntityFrameworkCore;

using OrderService.Application.EventHandlers;
using OrderService.Application.Interfaces;
using OrderService.Application.Services;
using OrderService.Application.Validators;
using OrderService.Domain.Events;
using OrderService.Domain.Interfaces;
using OrderService.Filters;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;

using SharedKernel.Events;
using SharedKernel.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Add global validation filter - validates all request bodies with FluentValidation
    options.Filters.Add<ValidationFilter>();
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register all FluentValidation validators from the Application assembly
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

// Add DbContext
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderDb")));

// Add repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Add application services
builder.Services.AddScoped<IOrderService, OrderApplicationService>();

// Add domain event dispatch
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddScoped<IDomainEventHandler<OrderPlaced>, OrderPlacedHandler>();
builder.Services.AddScoped<IDomainEventHandler<OrderConfirmed>, OrderConfirmedHandler>();
builder.Services.AddScoped<IDomainEventHandler<OrderCancelled>, OrderCancelledHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }