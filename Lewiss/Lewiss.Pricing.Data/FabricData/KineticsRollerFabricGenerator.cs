using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.OptionData;

namespace Lewiss.Pricing.Data.FabricData;

public record struct KineticsRollerFabricDataJSONStructure()
{
    public required string Fabric { get; init; }
    public required string Colour { get; init; }
    public required string Opacity { get; init; }
    public required decimal Multiplier { get; init; }
    public required int MaxWidth { get; init; }
    public required int MaxHeight { get; init; }
}
public static class KineticsRollerFabricGenerator
{

    private static int CurrentId = 0;

    private static int GetCurrentId()
    {
        return ++CurrentId;
    }

    public static string JSONFabricFilePath { get; } = "KineticsRollerFabricData.json";

    /// <summary>
    /// Reads JSON file and deserialises the data to <see cref="KineticsRollerFabricGenerator">. Data is then transformed to <see cref="KineticsRollerFabric"/> 
    /// </summary>
    /// <returns>
    /// A list of <see cref="KineticsRollerFabric"> model entities.
    /// </returns>
    public static List<KineticsRollerFabric> FabricList()
    {

        var jsonData = FabricDataUtility.GetJSONFileListData<KineticsRollerFabricDataJSONStructure>(JSONFabricFilePath);

        List<KineticsRollerFabric> fabricList = [];

        foreach (var fabric in jsonData)
        {
            var kineticsRollerFabric = new KineticsRollerFabric()
            {
                KineticsRollerFabricId = GetCurrentId(),
                Fabric = fabric.Fabric,
                Colour = fabric.Colour,
                Opacity = fabric.Opacity,
                Multiplier = fabric.Multiplier,
                MaxWidth = fabric.MaxWidth,
                MaxHeight = fabric.MaxHeight,
                ProductOptionVariationId = -1,
            };

            fabricList.Add(kineticsRollerFabric);
        }

        return fabricList;
    }

    public static List<ProductOptionVariation> GenerateProductOptionVariationList(List<KineticsRollerFabric> fabricList)
    {

        var fabricProductOptionId = FabricOption.ProductOption.ProductOptionId;

        List<ProductOptionVariation> productOptionVariationList = [];

        foreach (var fabric in fabricList)
        {
            var productOptionVariation = new ProductOptionVariation
            {
                ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
                Price = 0.00m,
                ProductOptionId = fabricProductOptionId,
                Value = fabric.GetFabricName,
            };

            productOptionVariationList.Add(productOptionVariation);
        }

        return productOptionVariationList;
    }

    public static (List<KineticsRollerFabric>, List<ProductOptionVariation>) LinkFabricListToProductOptionVariationList(List<KineticsRollerFabric> kineticRollerFabricList, List<ProductOptionVariation> productOptionVariationList)
    {
        if (kineticRollerFabricList.Count == 0)
        {
            throw new Exception("Kinetics Roller Fabric List is empty");
        }

        if (productOptionVariationList.Count == 0)
        {
            throw new Exception("Product Option Variation List is empty");
        }

        List<KineticsRollerFabric> fabricList = [];
        List<ProductOptionVariation> optionList = [];


        var productOptionVariationDictionary = productOptionVariationList.ToDictionary(po => po.Value, po => po);
        string key = "";
        bool result = false;

        foreach (var fabricModel in kineticRollerFabricList)
        {
            key = fabricModel.GetFabricName;
            result = productOptionVariationDictionary.TryGetValue(key, out ProductOptionVariation? currentProductOptionVariation);
            if (!result || currentProductOptionVariation is null)
            {
                throw new Exception("Failed to retrieve product option variation");
            }

            var linkedFabric = new KineticsRollerFabric
            {
                KineticsRollerFabricId = fabricModel.KineticsRollerFabricId,
                Fabric = fabricModel.Fabric,
                Colour = fabricModel.Colour,
                Opacity = fabricModel.Opacity,
                Multiplier = fabricModel.Multiplier,
                MaxWidth = fabricModel.MaxWidth,
                MaxHeight = fabricModel.MaxHeight,
                ProductOptionVariationId = currentProductOptionVariation.ProductOptionVariationId
            };

            fabricList.Add(linkedFabric);

            var linkedProductOptionVariation = new ProductOptionVariation
            {
                ProductOptionVariationId = currentProductOptionVariation.ProductOptionVariationId,
                ProductOptionId = currentProductOptionVariation.ProductOptionId,
                Price = currentProductOptionVariation.Price,
                Value = currentProductOptionVariation.Value,
            };

            optionList.Add(linkedProductOptionVariation);
        }


        return (fabricList, optionList);

    }

}