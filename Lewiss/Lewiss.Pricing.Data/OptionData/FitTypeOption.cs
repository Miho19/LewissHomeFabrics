using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class FitTypeOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "FitType"
    };

    public static readonly ProductOptionVariation FitTypeVariationInside = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Inside",
        Price = 0.0m,
    };

    public static readonly ProductOptionVariation FitTypeVariationOutside = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Outside",
        Price = 0.0m,
    };


    public static readonly List<ProductOptionVariation> OptionVariations = [FitTypeVariationInside, FitTypeVariationOutside];
}
