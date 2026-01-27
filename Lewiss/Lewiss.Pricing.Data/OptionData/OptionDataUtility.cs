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
        OperationTypeOption.ProductOption,
        OperationSideOption.ProductOption,
        HeadRailColourOption.ProductOption,
        SideChannelColourOption.ProductOption,
        RollTypeOption.ProductOption,
        ChainColourOption.ProductOption,
        ChainLengthOption.ProductOption,
        BracketTypeOption.ProductOption,
        BracketColourOption.ProductOption,
        BottomRailTypeOption.ProductOption,
        BottomRailColourOption.ProductOption,
        PelmetTypeOption.ProductOption,
        PelmetColourOption.ProductOption
    ];

    public static List<ProductOptionVariation> OptionVariationList = [
        ..FitTypeOption.ProductOptionVariations,
        ..FixingToOption.ProductOptionVariations,
        ..ProductTypeOption.ProductOptionVariations,
        ..FabricOption.ProductOptionVariations,
        ..OperationTypeOption.ProductOptionVariations,
        ..OperationSideOption.ProductOptionVariations,
        ..HeadRailColourOption.ProductOptionVariations,
        ..SideChannelColourOption.ProductOptionVariations,
        ..RollTypeOption.ProductOptionVariations,
        ..ChainColourOption.ProductOptionVariations,
        ..ChainLengthOption.ProductOptionVariations,
        ..BracketTypeOption.ProductOptionVariations,
        ..BracketColourOption.ProductOptionVariations,
        ..BottomRailTypeOption.ProductOptionVariations,
        ..BottomRailColourOption.ProductOptionVariations,
        ..PelmetTypeOption.ProductOptionVariations,
        ..PelmetColourOption.ProductOptionVariations
    ];
}


