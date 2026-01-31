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
    public async Task<IActionResult> GetFabrics([FromQuery] string fabricType, CancellationToken cancellationToken = default)
    {
        var fabricList = await _fabricService.GetFabricsAsync(fabricType, cancellationToken);
        if (fabricList is null || fabricList.Count == 0)
        {
            return StatusCode(404, new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = "Fabric list not found.",
            });
        }


        return new OkObjectResult(fabricList);
    }

    [HttpGet("{fabricName}", Name = "GetFabricPrice")]
    public async Task<IActionResult> GetFabricPrice(string fabricName, [FromQuery] int width, [FromQuery] int height, CancellationToken cancellationToken = default)
    {
        var fabricPrice = await _fabricService.GetFabricPriceAsync(fabricName, width, height, cancellationToken);

        if (fabricPrice == default)
        {
            return StatusCode(404, new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = "Fabric price not found.",
            });
        }


        return new OkObjectResult(fabricPrice);
    }


}