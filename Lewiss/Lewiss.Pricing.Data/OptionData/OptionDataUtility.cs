using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;


public static class OptionDataUtility
{
    private static int OptionId = 0;
    private static int OptionVariationId = 0;

    public static int GetOptionId()
    {
        return ++OptionId;
    }

    public static int GetOptionVariationId()
    {
        return ++OptionVariationId;
    }
    public static List<Option> OptionList = [
        FitTypeOption.Option,
        FixingToOption.Option,
        ProductTypeOption.Option,
        FabricOption.Option,
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
        ..ProductTypeOption.OptionVariations,
        ..FabricOption.OptionVariations,
    ];
}


