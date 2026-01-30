using Lewiss.Pricing.Data.FabricData;
using Xunit.Abstractions;

namespace Lewiss.Pricing.Api.Tests.Systems.Utility;

public class FabricDataUtilityTests
{
    private readonly ITestOutputHelper _logger;
    public FabricDataUtilityTests(ITestOutputHelper logger)
    {
        _logger = logger;
    }

    [Fact]
    public async Task GetFileData_ShouldReturnAString_WhenSuppliedAFileName()
    {
        var fileName = "KineticsCellularFabricData.json";
        var result = await FabricDataUtility.GetJSONFileListData<KineticsCellularFabricDataJSONStructure>(fileName);

        _logger.WriteLine($"output: {result}");

        Assert.NotNull(result);
        Assert.IsType<List<KineticsCellularFabricDataJSONStructure>>(result);
        Assert.NotEmpty(result);

    }

}