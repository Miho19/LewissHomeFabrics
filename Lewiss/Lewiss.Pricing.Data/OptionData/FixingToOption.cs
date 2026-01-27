using Lewiss.Pricing.Data.Model;

public static class FixingToOption
{
    public static readonly Option Option = new Option
    {
        Name = "FixingTo"
    };

    public static readonly OptionVariation FixingToWood = new OptionVariation
    {
        Option = Option,
        Value = "Wood",
        Price = 0.0m,
    };

    public static readonly List<OptionVariation> OptionVariations = [
        FixingToWood
    ];

}