using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric;
using Lewiss.Pricing.Data.OptionData;

namespace Lewiss.Pricing.Data.FabricData;

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

    public static string KineticsCellularJSONFabricDataFileName { get; } = "KineticsCellularFabricData.json";
    public static async Task<List<KineticsCellularFabric>> FabricListAsync(CancellationToken cancellationToken = default)
    {

        var jsonData = await FabricDataUtility.GetJSONFileListData<KineticsCellularFabricDataJSONStructure>(KineticsCellularJSONFabricDataFileName, cancellationToken);

        List<KineticsCellularFabric> fabricList = [];

        var fabricProductOptionId = FabricOption.ProductOption.ProductOptionId;

        foreach (var fabric in jsonData)
        {

            var fabricName = $"{fabric.Code} {fabric.Opacity} {fabric.Colour}";

            var productOptionVariation = new ProductOptionVariation
            {
                ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
                Price = 0.00m,
                ProductOptionId = fabricProductOptionId,
                Value = fabricName,
            };

            FabricOption.ProductOptionVariations.Add(productOptionVariation);

            var kineticsCellularFabric = new KineticsCellularFabric()
            {
                KineticsCellularFabricId = GetCurrentId(),
                Code = fabric.Code,
                Colour = fabric.Colour,
                Opacity = fabric.Opacity,
                Multiplier = fabric.Multiplier,
                ProductOptionVariationId = productOptionVariation.ProductOptionVariationId
            };

            fabricList.Add(kineticsCellularFabric);

        }

        return fabricList;
    }

}