using CustomerService.Application.Interfaces;
using CustomerService.Application.Services;
using CustomerService.Application.Validators;
using CustomerService.Domain.Interfaces;
using CustomerService.Filters;
using CustomerService.Infrastructure.Persistence;
using CustomerService.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
{
	// Add global validation filter - validates all request bodies with FluentValidation
	options.Filters.Add<ValidationFilter>();
});
builder.Services.AddOpenApi();

// Register all FluentValidation validators from the Application assembly
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();

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
