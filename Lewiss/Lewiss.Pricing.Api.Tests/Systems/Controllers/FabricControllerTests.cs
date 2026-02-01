using Lewiss.Pricing.Api.Controllers;
using Lewiss.Pricing.Api.Tests.Fixtures;
using Lewiss.Pricing.Data.Model.Fabric;
using Lewiss.Pricing.Shared.Fabric;
using Lewiss.Pricing.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object);
        var fabricController = new FabricController(fabricServiceMock.Object);
        var fabricType = "Kinetics Cellular";

        fabricServiceMock.Setup(f => f.GetFabricsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(FabricFixture.GetFabricListKineticsCellular);

        var result = await fabricController.GetFabrics(fabricType);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        var fabricList = Assert.IsAssignableFrom<List<IFabricDTO>>(okObjectResult.Value);
        Assert.NotEmpty(fabricList);

        var kineticsCellularFabricDTOList = fabricList.Cast<KineticsCellularFabricDTO>().ToList();

        foreach (var f in kineticsCellularFabricDTOList)
        {
            Assert.NotNull(f.Code);
        }
    }

    [Fact]
    public async Task GetFabricList_KineticsRollerQuery_ShouldReturn200Ok_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object);
        var fabricController = new FabricController(fabricServiceMock.Object);
        var fabricType = "Kinetics Roller";

        fabricServiceMock.Setup(f => f.GetFabricsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(FabricFixture.GetFabricListKineticsRoller);

        var result = await fabricController.GetFabrics(fabricType);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        var fabricList = Assert.IsAssignableFrom<List<IFabricDTO>>(okObjectResult.Value);
        Assert.NotEmpty(fabricList);

        var kineticsRollerFabricDTOList = fabricList.Cast<KineticsRollerFabricDTO>().ToList();

        foreach (var f in kineticsRollerFabricDTOList)
        {
            Assert.NotNull(f.Fabric);
        }
    }

    [Fact]
    public async Task GetFabricList_KineticsRollerQuery_ShouldReturn404Ntound_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object);
        var fabricController = new FabricController(fabricServiceMock.Object);
        var fabricType = "Kinetics Roller";

        fabricServiceMock.Setup(f => f.GetFabricsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var result = await fabricController.GetFabrics(fabricType);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
    }
}