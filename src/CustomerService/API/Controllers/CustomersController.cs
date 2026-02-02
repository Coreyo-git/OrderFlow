using CustomerService.Application.DTOs;
using CustomerService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
	private readonly ICustomerService _customerService;

	public CustomersController(ICustomerService customerService)
	{
		_customerService = customerService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
	{
		var customers = await _customerService.GetAllActiveAsync(cancellationToken);
		return Ok(customers);
	}

	[HttpGet("{id:guid}", Name = "GetCustomerById")]
	public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var customer = await _customerService.GetByIdAsync(id, cancellationToken);
		if (customer is null)
		{
			return NotFound();
		}
		return Ok(customer);
	}

	[HttpGet("by-email")]
	public async Task<IActionResult> GetByEmailAsync([FromQuery] string email, CancellationToken cancellationToken)
	{
		var customer = await _customerService.GetByEmailAsync(email, cancellationToken);
		if (customer is null)
		{
			return NotFound();
		}
		return Ok(customer);
	}

	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
	{
		var customer = await _customerService.CreateAsync(request, cancellationToken);
		return CreatedAtRoute("GetCustomerById", new { id = customer.Id }, customer);
	}

	[HttpGet("health")]
	public IActionResult Health() => Ok(new { status = "healthy" });
}