using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric;
using Lewiss.Pricing.Data.OptionData;

namespace Lewiss.Pricing.Data.FabricData;

// We can change this into generic generator and use a func delegate to populate the T 
public record struct KineticsCellularFabricDataJSONStructure()
{
    public required string Code;
    public required string Colour;
    public required string Opacity;
    public required int Multiplier;

}
public static class KineticsCellularFabricGenerator
{

    private static int CurrentId = 0;

    private static int GetCurrentId()
    {
        return ++CurrentId;
    }

    public static string JSONFilePath { get; } = "KineticsCellularFabricData.json";
    public static List<KineticsCellularFabric> FabricList()
    {

        var jsonData = FabricDataUtility.GetJSONFileListData<KineticsCellularFabricDataJSONStructure>(JSONFilePath);

        List<KineticsCellularFabric> fabricList = [];

        foreach (var fabric in jsonData)
        {

            var kineticsCellularFabric = new KineticsCellularFabric()
            {
                KineticsCellularFabricId = GetCurrentId(),
                Code = fabric.Code,
                Colour = fabric.Colour,
                Opacity = fabric.Opacity,
                Multiplier = fabric.Multiplier,
                ProductOptionVariationId = -1,
            };

            fabricList.Add(kineticsCellularFabric);

        }

        return fabricList;
    }

    public static List<ProductOptionVariation> GenerateProductOptionVariationList(List<KineticsCellularFabric> fabricList)
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

    public static (List<KineticsCellularFabric>, List<ProductOptionVariation>) LinkFabricListToProductOptionVariationList(List<KineticsCellularFabric> kineticCellularFabricList, List<ProductOptionVariation> productOptionVariationList)
    {
        if (kineticCellularFabricList.Count == 0)
        {
            throw new Exception("Kinetics Cellular Fabric List is empty");
        }

        if (productOptionVariationList.Count == 0)
        {
            throw new Exception("Product Option Variation List is empty");
        }

        List<KineticsCellularFabric> fabricList = [];
        List<ProductOptionVariation> optionList = [];


        var productOptionVariationDictionary = productOptionVariationList.ToDictionary(po => po.Value, po => po);
        string key = "";
        bool result = false;

        foreach (var fabricModel in kineticCellularFabricList)
        {
            key = fabricModel.GetFabricName;
            result = productOptionVariationDictionary.TryGetValue(key, out ProductOptionVariation? currentProductOptionVariation);
            if (!result || currentProductOptionVariation is null)
            {
                throw new Exception("Failed to retrieve product option variation");
            }

            var linkedFabric = new KineticsCellularFabric
            {
                KineticsCellularFabricId = fabricModel.KineticsCellularFabricId,
                Code = fabricModel.Code,
                Colour = fabricModel.Colour,
                Opacity = fabricModel.Opacity,
                Multiplier = fabricModel.Multiplier,
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