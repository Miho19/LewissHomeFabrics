using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.SeedData;

namespace Lewiss.Pricing.Data.OptionData;

public static class FixingToOption
{
    public static readonly Option Option = new Option
    {
        OptionId = OptionDataUtility.GetOptionId(),
        Name = "FixingTo"
    };

    public static readonly OptionVariation FixingToWood = new OptionVariation
    {
        OptionVariationId = OptionDataUtility.GetOptionVariationId(),
        OptionId = Option.OptionId,
        Value = "Wood",
        Price = 0.0m,
    };

    public static readonly List<OptionVariation> OptionVariations = [
        FixingToWood
    ];

}