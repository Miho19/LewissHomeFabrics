
using Lewiss.Pricing.Data.Model.Fabric.Price;

namespace Lewiss.Pricing.Data.FabricData;

public static class FabricPriceDataGenerator
{

    public static JSONPricingDataFileMeta LFJSONFile { get; } = new JSONPricingDataFileMeta()
    {
        FileName = "KineticsRollerLFPriceData.json",
        ProductType = "Kinetics Roller",
        Opacity = "LF"
    };

    public static JSONPricingDataFileMeta SSJSONFile { get; } = new JSONPricingDataFileMeta()
    {
        FileName = "KineticsRollerSSPriceData.json",
        ProductType = "Kinetics Roller",
        Opacity = "SS"
    };

    private static string[] FileList { get; } = [
        LFJSONFile.FileName,
        SSJSONFile.FileName
    ];

    private static JSONPricingDataStructure GetJSONPriceData(JSONPricingDataFileMeta fileMetaData)
    {
        var fileName = fileMetaData.FileName;

        if (!FileList.Contains(fileName))
        {
            throw new Exception("File is not part of Fabric Price Generator");
        }

        return FabricDataUtility.GetJSONFileData<JSONPricingDataStructure>(fileName);
    }

    public static List<FabricPrice> GetPriceModelList(JSONPricingDataFileMeta fileMetaData)
    {
        var pricingData = GetJSONPriceData(fileMetaData);
        return FabricPriceDataUtility.PricingDataStructureToFabricPrice(pricingData, fileMetaData.ProductType, fileMetaData.Opacity);
    }


}