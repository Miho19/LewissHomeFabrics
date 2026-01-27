using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;


public static class OptionDataUtility
{
    public static List<Option> OptionList = [
        FitTypeOption.Option,
        FixingToOption.Option,
        // ProductType,
        // Fabric,
        // OperationType,
        // OperationSide,
        // HeadRailColour,
        // SideChannelColour,
        // RollType,
        // ChainColour,
        // ChainLength,
        // BracketType,
        // BracketColour,
        // BottomRailType,
        // BottomRailColour,
        // PelmetType,
        // PelmetColour
    ];

    public static List<OptionVariation> OptionVariationList = [
        ..FitTypeOption.OptionVariations,
        ..FixingToOption.OptionVariations,

    ];
}


