using Castle.Core.Logging;
using FluentResults;
using Lewiss.Pricing.Api.Controllers;
using Lewiss.Pricing.Api.Tests.Fixtures;
using Lewiss.Pricing.Data.Model.Fabric;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.ProductStrategy;
using Lewiss.Pricing.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace Lewiss.Pricing.Api.Tests.Systems.Controllers;

public class FabricControllerTests
{
    private readonly ITestOutputHelper _logger;
    public FabricControllerTests(ITestOutputHelper logger)
    {
        _logger = logger;
    }

    [Fact]
    public async Task GetFabricList_KineticsCellularQuery_ShouldReturn200Ok_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);
        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);
        var fabricController = new FabricController(fabricServiceMock.Object);

        var fabricType = "Kinetics Cellular";

        fabricServiceMock.Setup(f => f.GetFabricsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(FabricFixture.GetFabricListKineticsCellular()));

        var result = await fabricController.GetFabrics(fabricType);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        var fabricList = Assert.IsAssignableFrom<List<FabricOutputDTO>>(okObjectResult.Value);
        Assert.NotEmpty(fabricList);

        var kineticsCellularFabricDTOList = fabricList.Cast<FabricOutputDTO>().ToList();

        foreach (var f in kineticsCellularFabricDTOList)
        {
            Assert.NotNull(f.Code);
        }
    }

    [Fact]
    public async Task GetFabricList_KineticsRollerQuery_ShouldReturn200Ok_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);
        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);
        var fabricController = new FabricController(fabricServiceMock.Object);

        var fabricType = "Kinetics Roller";

        fabricServiceMock.Setup(f => f.GetFabricsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(FabricFixture.GetFabricListKineticsRoller()));

        var result = await fabricController.GetFabrics(fabricType);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        var fabricList = Assert.IsAssignableFrom<List<FabricOutputDTO>>(okObjectResult.Value);
        Assert.NotEmpty(fabricList);

        var kineticsRollerFabricDTOList = fabricList.Cast<FabricOutputDTO>().ToList();

        foreach (var f in kineticsRollerFabricDTOList)
        {
            Assert.NotNull(f.Fabric);
        }
    }

    [Fact]
    public async Task GetFabricList_KineticsRollerQuery_ShouldReturn404Ntound_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);
        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);
        var fabricController = new FabricController(fabricServiceMock.Object);

        var fabricType = "Kinetics Roller";

        fabricServiceMock.Setup(f => f.GetFabricsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new Error("internal error")));

        var result = await fabricController.GetFabrics(fabricType);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Something went wrong", objectResult.Value.ToString());
    }
}