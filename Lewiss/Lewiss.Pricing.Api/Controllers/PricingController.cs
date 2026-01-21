using Microsoft.AspNetCore.Mvc;
using Lewiss.Pricing.Shared.Worksheet;
using Lewiss.Pricing.Shared.Customer;
using System;

namespace Lewiss.Pricing.Api.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class PricingController : ControllerBase
{
    public PricingController()
    {
        
    }
 
    [HttpPost("worksheet", Name = "CreateWorksheet")]

    public async Task<IActionResult> CreateWorksheet([FromBody] CustomerDTO customerDTO)
    {
        Guid worksheetId = Guid.CreateVersion7(DateTimeOffset.UtcNow);
        var workoutDTO = new WorksheetDTO()
        {
            WorksheetId = worksheetId,
            Customer = customerDTO,
            CreatedAt = new DateTimeOffset(DateTime.UtcNow, new TimeSpan(13,0, 0))
        };
        
        return new CreatedAtActionResult("Created Worksheet", nameof(CreateWorksheet), new {Id = worksheetId}, workoutDTO);
    }

}