using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class BracketTypeOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "BracketType"
    };

    public static readonly ProductOptionVariation Standard = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Standard",
        Price = 0.00m,
    };

    public static readonly ProductOptionVariation ExtraLarge = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Extra Large",
        Price = 0.00m,
    };

    public static readonly List<ProductOptionVariation> ProductOptionVariations = [
        Standard, ExtraLarge,
    ];
}