using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lewiss.Pricing.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;
    private readonly ILogger<CustomerController> _logger;
    public CustomerController(CustomerService customerService, ILogger<CustomerController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    [HttpGet("{customerId}", Name = "GetCustomer")]
    public async Task<IActionResult> GetCustomer(Guid customerId, CancellationToken cancellationToken = default)
    {
        var result = await _customerService.GetCustomerByExternalIdAsync(customerId, cancellationToken);
        if (result is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        if (result.IsFailed)
        {
            if (result.Errors.Any(e => e is NotFoundResource))
            {
                return NotFound("Customer not found.");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        var customerEntryDto = result.Value;


        return new OkObjectResult(customerEntryDto);
    }

    [HttpPost("", Name = "CreateCustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateInputDTO customerCreateDTO, CancellationToken cancellationToken = default)
    {
        var result = await _customerService.CreateCustomerAsync(customerCreateDTO, cancellationToken);

        if (result is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        if (result.IsFailed)
        {
            if (result.Errors.Any(e => e is CustomerAlreadyExists))
                return Conflict("Customer already exists");

            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        var customerEntryDto = result.Value;

        return new CreatedAtActionResult(actionName: "GetCustomer", controllerName: "Customer", routeValues: new { customerId = customerEntryDto.Id }, value: customerEntryDto);
    }

    [HttpGet("", Name = "GetCustomers")]
    public async Task<IActionResult> GetCustomers([FromQuery] GetCustomerQueryParameters getCustomerQueryParameters, CancellationToken cancellationToken = default)
    {
        var result = await _customerService.GetCustomersAsync(getCustomerQueryParameters, cancellationToken);

        if (result is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        if (result.IsFailed)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        var customerEntryDTOList = result.Value;

        return new OkObjectResult(customerEntryDTOList);
    }

}