using FluentResults;
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
        var result = await Result.Try(async Task<Result<CustomerEntryOutputDTO>> () => await _customerService.GetCustomerByExternalIdAsync(customerId, cancellationToken));

        if (result.IsFailed)
        {

            var validationError = result.Errors.OfType<ValidationError>().FirstOrDefault();
            if (validationError is not null)
            {
                return BadRequest(validationError.Message);
            }

            var notFoundResource = result.Errors.OfType<NotFoundResource>().FirstOrDefault();
            if (notFoundResource is not null)
            {
                return NotFound(notFoundResource.Message);
            }

            var resourceNotOwned = result.Errors.OfType<ResourceNotOwned>().FirstOrDefault();
            {
                if (resourceNotOwned is not null)
                {
                    return BadRequest(resourceNotOwned.Message);
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        var customerEntryDto = result.Value;


        return new OkObjectResult(customerEntryDto);
    }

    [HttpPost("", Name = "CreateCustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateInputDTO customerCreateDTO, CancellationToken cancellationToken = default)
    {
        var result = await Result.Try(async Task<Result<CustomerEntryOutputDTO>> () => await _customerService.CreateCustomerAsync(customerCreateDTO, cancellationToken));

        if (result.IsFailed)
        {

            var resourceAlreadyExists = result.Errors.OfType<CustomerAlreadyExists>().FirstOrDefault();
            if (resourceAlreadyExists is not null)
            {
                return Conflict("Customer already exists");
            }

            var validationError = result.Errors.OfType<ValidationError>().FirstOrDefault();
            if (validationError is not null)
            {
                return BadRequest(validationError.Message);
            }

            var notFoundResource = result.Errors.OfType<NotFoundResource>().FirstOrDefault();
            if (notFoundResource is not null)
            {
                return NotFound(notFoundResource.Message);
            }

            var resourceNotOwned = result.Errors.OfType<ResourceNotOwned>().FirstOrDefault();
            {
                if (resourceNotOwned is not null)
                {
                    return BadRequest(resourceNotOwned.Message);
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        var customerEntryDto = result.Value;

        return new CreatedAtActionResult(actionName: "GetCustomer", controllerName: "Customer", routeValues: new { customerId = customerEntryDto.Id }, value: customerEntryDto);
    }

    [HttpGet("", Name = "GetCustomers")]
    public async Task<IActionResult> GetCustomers([FromQuery] GetCustomerQueryParameters getCustomerQueryParameters, CancellationToken cancellationToken = default)
    {
        var result = await Result.Try(async Task<Result<List<CustomerEntryOutputDTO>>> () => await _customerService.GetCustomersAsync(getCustomerQueryParameters, cancellationToken));

        if (result.IsFailed)
        {

            var validationError = result.Errors.OfType<ValidationError>().FirstOrDefault();
            if (validationError is not null)
            {
                return BadRequest(validationError.Message);
            }

            var notFoundResource = result.Errors.OfType<NotFoundResource>().FirstOrDefault();
            if (notFoundResource is not null)
            {
                return NotFound(notFoundResource.Message);
            }

            var resourceNotOwned = result.Errors.OfType<ResourceNotOwned>().FirstOrDefault();
            {
                if (resourceNotOwned is not null)
                {
                    return BadRequest(resourceNotOwned.Message);
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        var customerEntryDTOList = result.Value;

        return new OkObjectResult(customerEntryDTOList);
    }

}