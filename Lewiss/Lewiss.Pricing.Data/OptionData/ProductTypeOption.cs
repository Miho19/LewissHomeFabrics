using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class ProductTypeOption
{
    public static readonly Option Option = new Option
    {
        OptionId = OptionDataUtility.GetOptionId(),
        Name = "ProductType"
    };

    public static readonly OptionVariation KineticsCellular = new OptionVariation
    {
        OptionVariationId = OptionDataUtility.GetOptionVariationId(),
        OptionId = Option.OptionId,
        Value = "Kinetics Cellular",
        Price = 0.0m,
    };

    public static readonly OptionVariation KineticsRoller = new OptionVariation
    {
        OptionVariationId = OptionDataUtility.GetOptionVariationId(),
        OptionId = Option.OptionId,
        Value = "Kinetics Roller",
        Price = 0.0m,
    };

    public static readonly List<OptionVariation> OptionVariations = [
        KineticsCellular, KineticsRoller
    ];
}