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

    [HttpGet("customer/{customerId}", Name = "GetCustomer")]
    public async Task<IActionResult> GetCustomer(Guid customerId, CancellationToken cancellationToken = default)
    {
        var customerEntryDTO = await _customerService.GetCustomerByExternalIdAsync(customerId, cancellationToken);
        if (customerEntryDTO is null)
        {
            return StatusCode(404, new ProblemDetails
            {
                Status = 404,
                Title = "Not Found",
                Detail = "Customer not found.",
            });
        }

        return new OkObjectResult(customerEntryDTO);
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

        return new CreatedAtActionResult(actionName: "GetCustomer", controllerName: "Pricing", routeValues: new { customerId = customerEntryDto.Id }, value: customerEntryDto);
    }

    [HttpGet("customer", Name = "GetCustomers")]
    public async Task<IActionResult> GetCustomers([FromQuery] GetCustomerQueryParameters getCustomerQueryParameters, CancellationToken cancellationToken = default)
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


    [HttpPost("customer/{customerId}/worksheet", Name = "CreateWorksheet")]
    public async Task<IActionResult> CreateWorksheet([FromBody] CustomerEntryDTO customerDTO, Guid customerId, CancellationToken cancellationToken = default)
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

    [HttpGet("customer/{customerId}/worksheet/{workoutId}", Name = "GetWorksheet")]
    public async Task<IActionResult> GetWorksheet(Guid customerId, Guid workoutId, CancellationToken cancellationToken = default)
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

    [HttpPost("customer/{customerId}/worksheet/{workoutId}/product", Name = "CreateProduct")]
    public async Task<IActionResult> CreateProduct(Guid customerId, Guid workoutId, [FromBody] ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
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

    [HttpGet("customer/{customerId}/worksheet", Name = "GetCustomerWorksheet")]
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




}