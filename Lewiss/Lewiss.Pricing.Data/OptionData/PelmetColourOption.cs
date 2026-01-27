using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class PelmetColourOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "PelmetColour"
    };

    public static readonly ProductOptionVariation White = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "White",
        Price = 0.00m,
    };

    public static readonly ProductOptionVariation Black = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Black",
        Price = 0.00m,
    };

    public static readonly List<ProductOptionVariation> ProductOptionVariations = [
        White, Black
    ];
}