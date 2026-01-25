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
        var currentDateTimeOffset = DateTimeOffset.UtcNow;

        Guid worksheetId = Guid.CreateVersion7(currentDateTimeOffset);
        var workoutDTO = new WorksheetDTO()
        {
            WorksheetId = worksheetId,
            Customer = customerDTO,
            Price = 0.00m,
            Additional = 0.00m,
        };
        
        return new CreatedAtActionResult("Created Worksheet", nameof(CreateWorksheet), new {Id = worksheetId}, workoutDTO);
    }

}