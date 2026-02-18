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

    private class FabricControllerMocks
    {
        public Mock<IUnitOfWork> UnitOfWorkMock;
        public Mock<SharedUtilityService> SharedUtilityServiceMock;

        public Mock<IServiceProvider> ServiceProviderMock;

        public Mock<ProductStrategyResolver> ProductStrategyResolverMock;

        public Mock<ILogger<FabricService>> FabricServiceLoggerMock;
        public Mock<FabricService> FabricServiceMock;
        public FabricController FabricController;



        public FabricControllerMocks()
        {
            UnitOfWorkMock = new();
            SharedUtilityServiceMock = new(UnitOfWorkMock.Object);
            ServiceProviderMock = new();
            ProductStrategyResolverMock = new(ServiceProviderMock.Object);
            FabricServiceLoggerMock = new();
            FabricServiceMock = new(UnitOfWorkMock.Object, SharedUtilityServiceMock.Object, ProductStrategyResolverMock.Object, FabricServiceLoggerMock.Object);
            FabricController = new(FabricServiceMock.Object);
        }
    }


    [Fact]
    public async Task GetFabricList_KineticsCellularQuery_ShouldReturn200Ok_OnSuccess()
    {
        var fabricControllerMocks = new FabricControllerMocks();

        var fabricType = "Kinetics Cellular";

        fabricControllerMocks.FabricServiceMock.Setup(f => f.GetFabricsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(FabricFixture.GetFabricListKineticsCellular()));

        var result = await fabricControllerMocks.FabricController.GetFabrics(fabricType);

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
        var fabricControllerMocks = new FabricControllerMocks();


        var fabricType = "Kinetics Roller";

        fabricControllerMocks.FabricServiceMock.Setup(f => f.GetFabricsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(FabricFixture.GetFabricListKineticsRoller()));

        var result = await fabricControllerMocks.FabricController.GetFabrics(fabricType);

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
        var fabricControllerMocks = new FabricControllerMocks();


        var fabricType = "Kinetics Roller";

        fabricControllerMocks.FabricServiceMock.Setup(f => f.GetFabricsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new Error("internal error")));

        var result = await fabricControllerMocks.FabricController.GetFabrics(fabricType);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Something went wrong", objectResult.Value.ToString());
    }
}