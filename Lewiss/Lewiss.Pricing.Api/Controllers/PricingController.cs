using Lewiss.Pricing.Shared.ProductDTO;
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

    private readonly ILogger<PricingController> _logger;

    public PricingController(PricingService pricingService, CustomerService customerService, ProductService productService, WorksheetService worksheetService, ILogger<PricingController> logger)
    {
        _pricingService = pricingService;
        _customerService = customerService;
        _productService = productService;
        _worksheetService = worksheetService;

        _logger = logger;

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



    [HttpGet("customer/{customerId}/worksheet/{worksheetId}", Name = "GetWorksheet")]
    public async Task<IActionResult> GetWorksheet(Guid customerId, Guid worksheetId, CancellationToken cancellationToken = default)
    {
        var worksheetDTO = await _worksheetService.GetWorksheetAsync(customerId, worksheetId, cancellationToken);
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


    [HttpPost("customer/{customerId}/worksheet", Name = "CreateWorksheet")]
    public async Task<IActionResult> CreateWorksheet(Guid customerId, CancellationToken cancellationToken = default)
    {
        var worksheetDTO = await _worksheetService.CreateWorksheetAsync(customerId, cancellationToken);
        if (worksheetDTO is null)
        {
            return new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "Failed to create worksheet"
            });
        }

        return new CreatedAtActionResult("GetWorksheet", "Pricing", new { worksheetId = worksheetDTO.Id, customerId }, worksheetDTO);
    }



    [HttpPost("customer/{customerId}/worksheet/{workoutId}/product", Name = "CreateProduct")]
    public async Task<IActionResult> CreateProduct(Guid customerId, Guid workoutId, [FromBody] ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {

        var productEntryDTO = await _productService.CreateProductAsync(customerId, workoutId, productCreateDTO, cancellationToken);
        if (productEntryDTO is null)
        {
            return new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "Failed to create product"
            });
        }

        return new OkObjectResult(productEntryDTO);
    }

    [HttpGet("customer/{customerId}/worksheet/{worksheetId}/product/{productId}", Name = "GetProduct")]
    public async Task<IActionResult> GetProduct(Guid customerId, Guid worksheetId, Guid productId, CancellationToken cancellationToken = default)
    {

        var productEntryDTO = await _productService.GetProductAsync(customerId, worksheetId, productId, cancellationToken);
        if (productEntryDTO is null)
        {
            return new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = "Product not found"
            });
        }

        return new OkObjectResult(productEntryDTO);
    }

    [HttpGet("customer/{customerId}/worksheet/{worksheetId}/product", Name = "GetWorksheetProduct")]
    public async Task<IActionResult> GetWorksheetProduct(Guid customerId, Guid worksheetId, CancellationToken cancellationToken = default)
    {

        var productEntryDTOList = await _worksheetService.GetWorksheetProductsAsync(customerId, worksheetId, cancellationToken);
        if (productEntryDTOList is null)
        {
            return new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "Something went wrong."
            });
        }

        return new OkObjectResult(productEntryDTOList);
    }

}