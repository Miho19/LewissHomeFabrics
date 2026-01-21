using Lewiss.Pricing.Api.Controllers;
using Lewiss.Pricing.Shared.Worksheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace Lewiss.Pricing.Api.Tests.Systems.Controllers;

public class PricingControllerTests
{
    private readonly ITestOutputHelper _logger;
    public PricingControllerTests(ITestOutputHelper logger)
    {
        _logger = logger;
    }

    [Fact]
    public async Task CreateWorksheet_ShouldReturnOkCreated_OnSuccess()
    {   
        var customerDTO = CustomerFixture.TestCustomer;
        
        var pricingController = new PricingController();
        var result = await pricingController.CreateWorksheet(customerDTO);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
        
        var worksheetDTO = Assert.IsType<WorksheetDTO>(createdAtActionResult.Value);
        Assert.Equal(customerDTO.FamilyName, worksheetDTO.Customer.FamilyName);

    }


}