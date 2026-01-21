using Microsoft.AspNetCore.Mvc;

namespace Lewiss.Pricing.Api.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class PricingController : ControllerBase
{
    public PricingController()
    {
        
    }
 
    [HttpPost("worksheet", Name = "CreateWorksheet")]

    public async Task<IActionResult> CreateWorksheet()
    {
        return new NotFoundObjectResult("");
    }

}