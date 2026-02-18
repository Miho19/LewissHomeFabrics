using FluentResults;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lewiss.Pricing.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FabricController : ControllerBase
{
    private readonly FabricService _fabricService;

    public FabricController(FabricService fabricService)
    {
        _fabricService = fabricService;
    }

    [HttpGet("", Name = "GetFabrics")]
    public async Task<IActionResult> GetFabrics([FromQuery] string productType, CancellationToken cancellationToken = default)
    {
        var result = await Result.Try(async Task<Result<List<FabricOutputDTO>>> () => await _fabricService.GetFabricsAsync(productType, cancellationToken));

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

        var fabricList = result.Value;
        return new OkObjectResult(fabricList);
    }

}