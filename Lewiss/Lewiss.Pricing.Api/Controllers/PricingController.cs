using FluentResults;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.Services;
using Lewiss.Pricing.Shared.WorksheetDTO;
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
        var result = await Result.Try(async Task<Result<List<WorksheetOutputDTO>>> () =>
        {
            return await _customerService.GetCustomerWorksheetDTOListAsync(customerId, cancellationToken);
        });

        if (result.IsFailed)
        {
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

        var worksheetDTOList = result.Value;

        return new OkObjectResult(worksheetDTOList);
    }



    [HttpGet("customer/{customerId}/worksheet/{worksheetId}", Name = "GetWorksheet")]
    public async Task<IActionResult> GetWorksheet(Guid customerId, Guid worksheetId, CancellationToken cancellationToken = default)
    {
        var result = await Result.Try(async Task<Result<WorksheetOutputDTO>> () =>
        {
            return await _worksheetService.GetWorksheetAsync(customerId, worksheetId, cancellationToken);
        });


        if (result.IsFailed)
        {
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

        var worksheetDTO = result.Value;

        return new OkObjectResult(worksheetDTO);
    }


    [HttpPost("customer/{customerId}/worksheet", Name = "CreateWorksheet")]
    public async Task<IActionResult> CreateWorksheet(Guid customerId, CancellationToken cancellationToken = default)
    {
        var result = await Result.Try(async Task<Result<WorksheetOutputDTO>> () =>
        {
            return await _worksheetService.CreateWorksheetAsync(customerId, cancellationToken);
        });

        if (result.IsFailed)
        {
            var notFoundResource = result.Errors.OfType<NotFoundResource>().FirstOrDefault();
            if (notFoundResource is not null)
            {
                return NotFound(notFoundResource.Message);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        var worksheetDTO = result.Value;

        return new CreatedAtActionResult("GetWorksheet", "Pricing", new { worksheetId = worksheetDTO.Id, customerId }, worksheetDTO);
    }



    [HttpPost("customer/{customerId}/worksheet/{workoutId}/product", Name = "CreateProduct")]
    public async Task<IActionResult> CreateProduct(Guid customerId, Guid workoutId, [FromBody] ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {

        var result = await Result.Try(async Task<Result<ProductEntryOutputDTO>> () =>
        {
            return await _productService.CreateProductAsync(customerId, workoutId, productCreateDTO, cancellationToken);
        });

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

        var productEntryDTO = result.Value;

        return new OkObjectResult(productEntryDTO);
    }

    [HttpGet("customer/{customerId}/worksheet/{worksheetId}/product/{productId}", Name = "GetProduct")]
    public async Task<IActionResult> GetProduct(Guid customerId, Guid worksheetId, Guid productId, CancellationToken cancellationToken = default)
    {
        var result = await Result.Try(async Task<Result<ProductEntryOutputDTO>> () =>
        {
            return await _productService.GetProductAsync(customerId, worksheetId, productId, cancellationToken);
        });

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

        var productEntryDTO = result.Value;

        return new OkObjectResult(productEntryDTO);
    }

    [HttpGet("customer/{customerId}/worksheet/{worksheetId}/product", Name = "GetWorksheetProduct")]
    public async Task<IActionResult> GetWorksheetProduct(Guid customerId, Guid worksheetId, CancellationToken cancellationToken = default)
    {
        var result = await Result.Try(async Task<Result<List<ProductEntryOutputDTO>>> () =>
        {
            return await _worksheetService.GetWorksheetProductsAsync(customerId, worksheetId, cancellationToken);
        });

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

        var productEntryDTOList = result.Value;
        return new OkObjectResult(productEntryDTOList);
    }

}