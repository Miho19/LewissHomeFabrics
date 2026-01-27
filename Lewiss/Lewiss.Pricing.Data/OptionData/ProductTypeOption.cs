using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class ProductTypeOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "ProductType"
    };

    public static readonly ProductOptionVariation KineticsCellular = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Kinetics Cellular",
        Price = 0.0m,
    };

    public static readonly ProductOptionVariation KineticsRoller = new ProductOptionVariation
    {
        ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
        ProductOptionId = ProductOption.ProductOptionId,
        Value = "Kinetics Roller",
        Price = 0.0m,
    };

    public static readonly List<ProductOptionVariation> OptionVariations = [
        KineticsCellular, KineticsRoller
    ];
}