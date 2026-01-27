using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class FabricOption
{
    public static readonly Option Option = new Option
    {
        OptionId = OptionDataUtility.GetOptionId(),
        Name = "Fabric"
    };

    public static readonly OptionVariation TranslucentWhite = new OptionVariation
    {
        OptionVariationId = OptionDataUtility.GetOptionVariationId(),
        OptionId = Option.OptionId,
        Value = "Translucent White",
        Price = 0.0m,
    };

    public static readonly OptionVariation EverydayVinylCollectionPolar = new OptionVariation
    {
        OptionVariationId = OptionDataUtility.GetOptionVariationId(),
        OptionId = Option.OptionId,
        Value = "Everyday Vinyl Collection - Polar",
        Price = 0.0m,
    };

    public static readonly List<OptionVariation> OptionVariations = [
        TranslucentWhite, EverydayVinylCollectionPolar
    ];
}