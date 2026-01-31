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
    public async Task GetJSONFileListData_ShouldReturnJSONData_WhenSuppliedKineticsRollerFabricJSONFileData()
    {
        var fileName = KineticsRollerFabricGenerator.JSONFabricFilePath;
        var result = FabricDataUtility.GetJSONFileListData<KineticsRollerFabricDataJSONStructure>(fileName);

        Assert.NotNull(result);
        Assert.IsType<List<KineticsRollerFabricDataJSONStructure>>(result);
        Assert.NotEmpty(result);

    }


}