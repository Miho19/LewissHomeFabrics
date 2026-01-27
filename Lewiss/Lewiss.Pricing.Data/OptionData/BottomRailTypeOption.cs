using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class BottomRailTypeOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "BottomRailType"
    };

    public static readonly ProductOptionVariation Flat = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Flat",
        Price = 0.00m,
    };

    public static readonly ProductOptionVariation Deluxe = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Deluxe",
        Price = 25.00m,
    };

    public static readonly List<ProductOptionVariation> ProductOptionVariations = [
        Flat, Deluxe,
    ];
}