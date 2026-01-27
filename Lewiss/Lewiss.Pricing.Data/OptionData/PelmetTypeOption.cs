using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class PelmetTypeOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "PelmetType"
    };

    public static readonly ProductOptionVariation None = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "None",
        Price = 0.00m,
    };

    public static readonly List<ProductOptionVariation> ProductOptionVariations = [
        None,
    ];
}