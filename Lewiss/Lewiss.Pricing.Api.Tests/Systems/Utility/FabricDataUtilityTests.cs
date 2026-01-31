using System.Text.RegularExpressions;
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
        var result = FabricDataUtility.GetJSONFileData<List<KineticsRollerFabricDataJSONStructure>>(fileName);

        Assert.NotNull(result);
        Assert.IsType<List<KineticsRollerFabricDataJSONStructure>>(result);
        Assert.NotEmpty(result);

    }

    [Fact]
    public async Task KineticsRollerFabricPriceDataGenerator_GetPriceModelList_ShouldThrow_WhenSuppliedIncorrectPath()
    {
        var fileName = "random-file.json";
        Assert.Throws<Exception>(() => KineticsRollerFabricPriceDataGenerator.GetPriceModelList(fileName));
    }

    [Fact]
    public async Task KineticsRollerFabricPriceDataGenerator_GetPriceModelList_ShouldReturnAList_WhenSuppliedCorrectPath()
    {
        var fileName = KineticsRollerFabricPriceDataGenerator.LFJSONPriceDataFileName;

        Assert.Throws<Exception>(() => KineticsRollerFabricPriceDataGenerator.GetPriceModelList(fileName));
    }




}