using Lewiss.Pricing.Data.FabricData;
using Lewiss.Pricing.Data.Model.Fabric.Price;
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
    public async Task FabricPriceDataGenerator_GetPriceModelList_ShouldThrow_WhenSuppliedIncorrectPath()
    {

        Assert.Throws<Exception>(() => FabricPriceDataGenerator.GetPriceModelList(new JSONPricingDataFileMeta
        {
            FileName = "Random Name.txt",
            Opacity = "Fake",
            ProductType = "fake",
        }));
    }

    [Theory]
    [InlineData(1200, 900)]
    [InlineData(3100, 3000)]
    [InlineData(240, 240)]
    [InlineData(240, 900)]
    [InlineData(2400, 900)]
    [InlineData(2400, 1200)]
    public async Task FabricPriceDataGenerator_GetPriceModelList_ShouldReturnAList_KineticsRollerLF(int width, int height)
    {
        var file = FabricPriceDataGenerator.KineticsRollerLFJSONFile;

        var result = FabricPriceDataGenerator.GetPriceModelList(file);

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var pricing = Assert.IsType<List<FabricPrice>>(result);

        var price = pricing.FirstOrDefault(p => p.Height == height && p.Width == width);

        Assert.NotNull(price);
    }


    [Theory]
    [InlineData(1200, 900)]
    [InlineData(3100, 3000)]
    [InlineData(240, 240)]
    [InlineData(240, 900)]
    [InlineData(2400, 900)]
    [InlineData(2400, 1200)]
    public async Task FabricPriceDataGenerator_GetPriceModelList_ShouldReturnAList_KineticsRollerSS(int width, int height)
    {
        var file = FabricPriceDataGenerator.KineticsRollerSSJSONFile;

        var result = FabricPriceDataGenerator.GetPriceModelList(file);

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var pricing = Assert.IsType<List<FabricPrice>>(result);

        var price = pricing.FirstOrDefault(p => p.Height == height && p.Width == width);

        Assert.NotNull(price);
    }

    [Theory]
    [InlineData(1200, 900)]
    [InlineData(1600, 3600)]
    [InlineData(300, 300)]
    [InlineData(300, 3600)]
    [InlineData(1700, 900)]
    [InlineData(2200, 2700)]
    public async Task FabricPriceDataGenerator_GetPriceModelList_ShouldReturnAList_KineticsCellularTranslucent(int width, int height)
    {
        var file = FabricPriceDataGenerator.KineticsCellularTranslucentJSONFile;

        var result = FabricPriceDataGenerator.GetPriceModelList(file);

        Assert.NotNull(result);
        Assert.NotEmpty(result);

        var pricing = Assert.IsType<List<FabricPrice>>(result);

        var price = pricing.FirstOrDefault(p => p.Height == height && p.Width == width);

        Assert.NotNull(price);
    }

}