using Microsoft.AspNetCore.Mvc;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.Services.Pricing;
using Lewiss.Pricing.Shared.QueryParameters;

namespace Lewiss.Pricing.Api.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class PricingController : ControllerBase
{
    private readonly PricingService _pricingService;

    public PricingController(PricingService pricingService)
    {
        _pricingService = pricingService;    
    }
 
    [HttpPost("worksheet", Name = "CreateWorksheet")]
    public async Task<IActionResult> CreateWorksheet([FromBody] CustomerEntryDTO customerDTO, CancellationToken cancellationToken = default)
    {
        var worksheet = await _pricingService.CreateWorksheetAsync(customerDTO, cancellationToken);
        if (worksheet is null)
        {
            return new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "Failed to create worksheet"
            });
        }
        
        return new CreatedAtActionResult("Created Worksheet", nameof(CreateWorksheet), new {Id = worksheet.Id}, worksheet);
    }

    [HttpPost("customer", Name = "CreateCustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDTO customerCreateDTO, CancellationToken cancellationToken = default)
    {
        var customerEntryDto = await _pricingService.CreateCustomerAsync(customerCreateDTO, cancellationToken);
        if (customerEntryDto is null)
        {
            return StatusCode(500, new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = "Failed to create customer",
            });
        }

        return new CreatedAtActionResult("Created Customer", nameof(CreateCustomer), new {Id = customerEntryDto.Id}, customerEntryDto);
    }

    [HttpGet("customer", Name = "GetCustomer")]
    public async Task<IActionResult> GetCustomer([FromQuery] GetCustomerQueryParameters getCustomerQueryParameters, CancellationToken cancellationToken = default)
    {
        var customerEntryDTOList = await _pricingService.GetCustomersAsync(getCustomerQueryParameters, cancellationToken);
        if(customerEntryDTOList is null)
        {
            return StatusCode(500, new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = "Failed to retrieve customer",
            });
        }
        
        return new OkObjectResult(customerEntryDTOList);
    }
}