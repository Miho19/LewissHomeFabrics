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
    public static List<ProductOption> OptionList = [
        FitTypeOption.ProductOption,
        FixingToOption.ProductOption,
        ProductTypeOption.ProductOption,
        FabricOption.ProductOption,
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

    public static List<ProductOptionVariation> OptionVariationList = [
        ..FitTypeOption.OptionVariations,
        ..FixingToOption.OptionVariations,
        ..ProductTypeOption.OptionVariations,
        ..FabricOption.OptionVariations,
    ];
}


