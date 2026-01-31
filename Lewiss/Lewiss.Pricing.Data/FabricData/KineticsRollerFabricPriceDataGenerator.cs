
using Lewiss.Pricing.Data.Model.Fabric.Price;

namespace Lewiss.Pricing.Data.FabricData;

public static class KineticsRollerFabricPriceDataGenerator
{
    public static string LFJSONPriceDataFileName { get; } = "KineticsRollerLFPriceData.json";
    public static string SSJSONPriceDataFileName { get; } = "KineticsRollerSSPriceData.json";

    public static string[] FileList { get; } = [
        LFJSONPriceDataFileName,
        SSJSONPriceDataFileName
    ];

    private static JSONPricingDataStructure GetJSONPriceData(string fileName)
    {
        if (!FileList.Contains(fileName))
        {
            throw new Exception("File name does not belong to this class");
        }

        return FabricDataUtility.GetJSONFileData<JSONPricingDataStructure>(fileName);
    }

    public static List<FabricPrice> GetPriceModelList(string fileName)
    {
        var pricingData = GetJSONPriceData(fileName);

        return FabricPriceDataUtility.PricingDataStructureToFabricPrice(pricingData);
    }


}