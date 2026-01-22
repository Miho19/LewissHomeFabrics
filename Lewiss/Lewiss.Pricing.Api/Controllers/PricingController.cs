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

    /** 
        Add Customer To database / 
        Request new Worksheet from Database
    */
    public async Task<IActionResult> CreateWorksheet([FromBody] CustomerDTO customerDTO)
    {
        var currentDateTimeOffset = DateTimeOffset.UtcNow;

        Guid worksheetId = Guid.CreateVersion7(currentDateTimeOffset);
        var workoutDTO = new WorksheetDTO()
        {
            WorksheetId = worksheetId,
            Customer = customerDTO,
            CreatedAt = currentDateTimeOffset
        };
        
        return new CreatedAtActionResult("Created Worksheet", nameof(CreateWorksheet), new {Id = worksheetId}, workoutDTO);
    }

}