using Microsoft.AspNetCore.Mvc;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.Services.Pricing;

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
    public async Task<IActionResult> CreateWorksheet([FromBody] CustomerEntryDTO customerDTO)
    {
        var worksheet = await _pricingService.CreateWorksheet(customerDTO);
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
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDTO customerCreateDTO)
    {
        var customerEntryDto = await _pricingService.CreateCustomer(customerCreateDTO);
        if (customerEntryDto is null)
        {
            return new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "Failed to create customer"
            });
        }

        return new CreatedAtActionResult("Created Customer", nameof(CreateCustomer), new {Id = customerEntryDto.Id}, customerEntryDto);
    } 

}