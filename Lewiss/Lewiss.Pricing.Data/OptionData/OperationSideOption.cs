using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class OperationSideOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "OpertionSide"
    };

    public static readonly ProductOptionVariation Left = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Left",
        Price = 0.00m,
    };

    public static readonly ProductOptionVariation Right = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Right",
        Price = 0.00m,
    };


    public static readonly List<ProductOptionVariation> ProductOptionVariations = [
        Left, Right
    ];
}