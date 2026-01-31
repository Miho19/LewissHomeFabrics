using Lewiss.Pricing.Api.Controllers;
using Lewiss.Pricing.Data.Model.Fabric;
using Lewiss.Pricing.Shared.Fabric;
using Moq;

namespace Lewiss.Pricing.Api.Tests.Systems.Controllers;

public class FabricControllerTests
{
    public FabricControllerTests()
    {

    }

    public async Task GetFabricList_ShouldReturn200Ok_OnSuccess()
    {
        var fabricServiceMock = new Mock<FabricService>();
        var fabricController = new FabricController(fabricServiceMock.Object);

        var fabricType = "Kinetics Cellular";
        var result = await fabricController.GetFabrics(fabricType);

        Assert.NotNull(result);
        var fabricList = Assert.IsType<List<KineticsCellularFabricDTO>>(result);
        Assert.NotEmpty(fabricList);
    }
}