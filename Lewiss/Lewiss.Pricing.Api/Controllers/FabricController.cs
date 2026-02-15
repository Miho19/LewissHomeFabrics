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
        var fabricList = await _fabricService.GetFabricsAsync(productType, cancellationToken);
        return new OkObjectResult(fabricList);
    }

    // [HttpGet("{productType}", Name = "GetFabricPrice")]
    // public async Task<IActionResult> GetFabricPrice(string productType, [FromQuery] GetFabricQueryParameters queryParameters, CancellationToken cancellationToken = default)
    // {
    //     var fabricPriceDTO = await _fabricService.GetFabricPriceAsync(productType, queryParameters, cancellationToken);
    //     return new OkObjectResult(fabricPriceDTO);
    // }


}