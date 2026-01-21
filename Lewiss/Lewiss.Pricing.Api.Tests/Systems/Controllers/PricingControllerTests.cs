using Lewiss.Pricing.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

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
        var pricingController = new PricingController();
        var result = await pricingController.CreateWorksheet();

        Assert.NotNull(result);
        var okResultObject = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, okResultObject.StatusCode);
    }


}