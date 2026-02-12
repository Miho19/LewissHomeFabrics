using Lewiss.Pricing.Shared.CustomerDTO;
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
        var customerEntryDTO = await _customerService.GetCustomerByExternalIdAsync(customerId, cancellationToken);
        if (customerEntryDTO is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = "Customer not found.",
            });
        }

        return new OkObjectResult(customerEntryDTO);
    }

    [HttpPost("", Name = "CreateCustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateInputDTO customerCreateDTO, CancellationToken cancellationToken = default)
    {
        var customerEntryDto = await _customerService.CreateCustomerAsync(customerCreateDTO, cancellationToken);
        if (customerEntryDto is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "Failed to create customer",
            });

        }

        return new CreatedAtActionResult(actionName: "GetCustomer", controllerName: "Customer", routeValues: new { customerId = customerEntryDto.Id }, value: customerEntryDto);
    }

    [HttpGet("", Name = "GetCustomers")]
    public async Task<IActionResult> GetCustomers([FromQuery] GetCustomerQueryParameters getCustomerQueryParameters, CancellationToken cancellationToken = default)
    {
        var customerEntryDTOList = await _customerService.GetCustomersAsync(getCustomerQueryParameters, cancellationToken);
        if (customerEntryDTOList is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "Failed to retrieve customer",
            });
        }

        return new OkObjectResult(customerEntryDTOList);
    }

}