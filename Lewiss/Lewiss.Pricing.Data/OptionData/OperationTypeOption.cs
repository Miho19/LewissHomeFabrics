using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class OperationTypeOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "OperationType"
    };

    public static readonly ProductOptionVariation LithiumIon = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Lithium-ion",
        Price = 250.0m,
    };

    public static readonly ProductOptionVariation Cord = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Cord",
        Price = 0.0m,
    };
    public static readonly ProductOptionVariation Chain = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Chain",
        Price = 0.0m,
    };

    public static readonly List<ProductOptionVariation> ProductOptionVariations = [
        LithiumIon, Cord, Chain
    ];
}