using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lewiss.Pricing.Api.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class PricingController : ControllerBase
{
    private readonly CustomerService _customerService;
    private readonly ProductService _productService;
    private readonly WorksheetService _worksheetService;

    public PricingController(CustomerService customerService, ProductService productService, WorksheetService worksheetService)
    {
        _customerService = customerService;
        _productService = productService;
        _worksheetService = worksheetService;
    }



    [HttpGet("customer/{customerId}/worksheet", Name = "GetCustomerWorksheet")]
    public async Task<IActionResult> GetCustomerWorksheet(Guid customerId, CancellationToken cancellationToken = default)
    {
        var result = await _customerService.GetCustomerWorksheetDTOListAsync(customerId, cancellationToken);

        if (result.IsFailed)
        {
            if (result.Errors.Any(e => e is NotFoundResource))
            {
                return NotFound("Customer not found.");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");

        }

        var worksheetDTOList = result.Value;

        return new OkObjectResult(worksheetDTOList);
    }



    [HttpGet("customer/{customerId}/worksheet/{worksheetId}", Name = "GetWorksheet")]
    public async Task<IActionResult> GetWorksheet(Guid customerId, Guid worksheetId, CancellationToken cancellationToken = default)
    {
        var worksheetDTO = await _worksheetService.GetWorksheetAsync(customerId, worksheetId, cancellationToken);

        return new OkObjectResult(worksheetDTO);
    }


    [HttpPost("customer/{customerId}/worksheet", Name = "CreateWorksheet")]
    public async Task<IActionResult> CreateWorksheet(Guid customerId, CancellationToken cancellationToken = default)
    {
        var worksheetDTO = await _worksheetService.CreateWorksheetAsync(customerId, cancellationToken);

        return new CreatedAtActionResult("GetWorksheet", "Pricing", new { worksheetId = worksheetDTO.Id, customerId }, worksheetDTO);
    }



    [HttpPost("customer/{customerId}/worksheet/{workoutId}/product", Name = "CreateProduct")]
    public async Task<IActionResult> CreateProduct(Guid customerId, Guid workoutId, [FromBody] ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {

        var productEntryDTO = await _productService.CreateProductAsync(customerId, workoutId, productCreateDTO, cancellationToken);

        return new OkObjectResult(productEntryDTO);
    }

    [HttpGet("customer/{customerId}/worksheet/{worksheetId}/product/{productId}", Name = "GetProduct")]
    public async Task<IActionResult> GetProduct(Guid customerId, Guid worksheetId, Guid productId, CancellationToken cancellationToken = default)
    {
        var productEntryDTO = await _productService.GetProductAsync(customerId, worksheetId, productId, cancellationToken);
        return new OkObjectResult(productEntryDTO);
    }

    [HttpGet("customer/{customerId}/worksheet/{worksheetId}/product", Name = "GetWorksheetProduct")]
    public async Task<IActionResult> GetWorksheetProduct(Guid customerId, Guid worksheetId, CancellationToken cancellationToken = default)
    {
        var productEntryDTOList = await _worksheetService.GetWorksheetProductsAsync(customerId, worksheetId, cancellationToken);
        return new OkObjectResult(productEntryDTOList);
    }

}