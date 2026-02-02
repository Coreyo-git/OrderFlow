using CustomerService.Application.Interfaces;
using CustomerService.Application.Services;
using CustomerService.Domain.Interfaces;
using CustomerService.Infrastructure.Persistence;
using CustomerService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<CustomerDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("CustomerDb")));

// Add repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Add application services
builder.Services.AddScoped<ICustomerService, CustomerApplicationService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
