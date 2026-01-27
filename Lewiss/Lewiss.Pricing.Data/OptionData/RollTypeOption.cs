using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class RollTypeOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "RollType"
    };

    public static readonly ProductOptionVariation Front = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Front",
        Price = 0.00m,
    };

    public static readonly ProductOptionVariation Back = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Back",
        Price = 0.00m,
    };

    public static readonly List<ProductOptionVariation> ProductOptionVariations = [
        Front, Back
    ];
}