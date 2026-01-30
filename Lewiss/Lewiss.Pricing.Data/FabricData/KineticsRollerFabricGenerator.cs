using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric;
using Lewiss.Pricing.Data.OptionData;

namespace Lewiss.Pricing.Data.FabricData;

public record struct KineticsRollerFabricDataJSONStructure()
{
    public required string Fabric;
    public required string Colour;
    public required string Opacity;
    public required int Multiplier;
    public required int MaxWidth;
    public required int MaxHeight;
}
public static class KineticsRollerFabricGenerator
{

    private static int CurrentId = 0;

    private static int GetCurrentId()
    {
        return ++CurrentId;
    }

    public static string JSONFabricFilePath { get; } = "KineticsRollerFabricData.json";
    public static async Task<List<KineticsRollerFabric>> FabricListAsync(CancellationToken cancellationToken = default)
    {

        var jsonData = await FabricDataUtility.GetJSONFileListData<KineticsRollerFabricDataJSONStructure>(JSONFabricFilePath, cancellationToken);

        List<KineticsRollerFabric> fabricList = [];

        var fabricProductOptionId = FabricOption.ProductOption.ProductOptionId;

        foreach (var fabric in jsonData)
        {

            var fabricName = $"{fabric.Fabric} {fabric.Colour} {fabric.Opacity}"; //

            var productOptionVariation = new ProductOptionVariation
            {
                ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
                Price = 0.00m,
                ProductOptionId = fabricProductOptionId,
                Value = fabricName,
            };

            FabricOption.ProductOptionVariations.Add(productOptionVariation);

            var kineticsRollerFabric = new KineticsRollerFabric()
            {
                KineticsRollerFabricId = GetCurrentId(),
                Fabric = fabric.Fabric,
                Colour = fabric.Colour,
                Opacity = fabric.Opacity,
                Multiplier = fabric.Multiplier,
                MaxWidth = fabric.MaxWidth,
                MaxHeight = fabric.MaxHeight,
                ProductOptionVariationId = productOptionVariation.ProductOptionVariationId
            };

            fabricList.Add(kineticsRollerFabric);

        }

        return fabricList;
    }

}