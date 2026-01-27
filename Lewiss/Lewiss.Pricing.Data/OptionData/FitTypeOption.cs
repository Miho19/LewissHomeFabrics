using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class FitTypeOption
{
    public static readonly Option Option = new Option
    {
        Name = "FitType"
    };

    public static readonly OptionVariation FitTypeVariationInside = new OptionVariation
    {
        Option = Option,
        Value = "Inside",
        Price = 0.0m,
    };

    public static readonly OptionVariation FitTypeVariationOutside = new OptionVariation
    {
        Option = Option,
        Value = "Outside",
        Price = 0.0m,
    };


    public static readonly List<OptionVariation> OptionVariations = [FitTypeVariationInside, FitTypeVariationOutside];
}
