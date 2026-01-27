using System.Collections.Immutable;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.Product;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lewiss.Pricing.Api.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class PricingController : ControllerBase
{
    private readonly PricingService _pricingService;
    private readonly CustomerService _customerService;
    private readonly ProductService _productService;
    private readonly WorksheetService _worksheetService;

    public PricingController(PricingService pricingService, CustomerService customerService, ProductService productService, WorksheetService worksheetService)
    {
        _pricingService = pricingService;
        _customerService = customerService;
        _productService = productService;
        _worksheetService = worksheetService;

    }

    [HttpPost("worksheet", Name = "CreateWorksheet")]
    public async Task<IActionResult> CreateWorksheet([FromBody] CustomerEntryDTO customerDTO, CancellationToken cancellationToken = default)
    {
        var worksheet = await _worksheetService.CreateWorksheetAsync(customerDTO, cancellationToken);
        if (worksheet is null)
        {
            return new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "Failed to create worksheet"
            });
        }

        return new CreatedAtActionResult("Created Worksheet", nameof(CreateWorksheet), new { Id = worksheet.Id }, worksheet);
    }

    [HttpGet("worksheet/{workoutId}", Name = "GetWorksheet")]
    public async Task<IActionResult> GetWorksheet(Guid workoutId, CancellationToken cancellationToken = default)
    {
        var worksheetDTO = await _worksheetService.GetWorksheetDTOAsync(workoutId, cancellationToken);
        if (worksheetDTO is null)
        {
            return new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Internal Server Error",
                Detail = "Worksheet Not Found"
            });
        }

        return new OkObjectResult(worksheetDTO);
    }

    [HttpPost("worksheet/{workoutId}/product", Name = "CreateProduct")]
    public async Task<IActionResult> CreateProduct(Guid workoutId, [FromBody] ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        var productEntryDTO = await _productService.CreateProductAsync(workoutId, productCreateDTO, cancellationToken);
        if (productEntryDTO is null)
        {
            return new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Internal Server Error",
                Detail = "Failed to create product"
            });
        }

        return new OkObjectResult(productEntryDTO);
    }

    [HttpGet("customer/{customerId}", Name = "GetCustomerWorksheet")]
    public async Task<IActionResult> GetCustomerWorksheet(Guid customerId, CancellationToken cancellationToken = default)
    {
        var worksheetDTOList = await _customerService.GetCustomerWorksheetDTOListAsync(customerId, cancellationToken);
        if (worksheetDTOList is null)
        {
            return new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "Failed to retrieve worksheet"
            });
        }

        return new OkObjectResult(worksheetDTOList);
    }

    [HttpPost("customer", Name = "CreateCustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDTO customerCreateDTO, CancellationToken cancellationToken = default)
    {
        var customerEntryDto = await _customerService.CreateCustomerAsync(customerCreateDTO, cancellationToken);
        if (customerEntryDto is null)
        {
            return StatusCode(500, new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = "Failed to create customer",
            });
        }

        return new CreatedAtActionResult("Created Customer", nameof(CreateCustomer), new { Id = customerEntryDto.Id }, customerEntryDto);
    }

    [HttpGet("customer", Name = "GetCustomer")]
    public async Task<IActionResult> GetCustomer([FromQuery] GetCustomerQueryParameters getCustomerQueryParameters, CancellationToken cancellationToken = default)
    {
        var customerEntryDTOList = await _customerService.GetCustomersAsync(getCustomerQueryParameters, cancellationToken);
        if (customerEntryDTOList is null)
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