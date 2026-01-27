using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class FixingToOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "FixingTo"
    };

    public static readonly ProductOptionVariation FixingToWood = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Wood",
        Price = 0.0m,
    };

    public static readonly List<ProductOptionVariation> OptionVariations = [
        FixingToWood
    ];

}