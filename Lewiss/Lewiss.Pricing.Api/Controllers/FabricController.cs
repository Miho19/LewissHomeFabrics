using Lewiss.Pricing.Shared.CustomError;
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
        var result = await _fabricService.GetFabricsAsync(productType, cancellationToken);
        if (result is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }
        if (result.IsFailed)
        {
            if (result.Errors.Any(e => e is ValidationError))
            {
                return BadRequest("Product Type is invalid");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
        }

        var fabricList = result.Value;
        return new OkObjectResult(fabricList);
    }

}