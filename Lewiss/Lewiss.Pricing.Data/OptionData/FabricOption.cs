using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class FabricOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "Fabric"
    };

    public static readonly ProductOptionVariation TranslucentWhite = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Translucent White",
        Price = 0.0m,
    };

    public static readonly ProductOptionVariation EverydayVinylCollectionPolar = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Everyday Vinyl Collection - Polar",
        Price = 0.0m,
    };

    public static readonly List<ProductOptionVariation> OptionVariations = [
        TranslucentWhite, EverydayVinylCollectionPolar
    ];
}