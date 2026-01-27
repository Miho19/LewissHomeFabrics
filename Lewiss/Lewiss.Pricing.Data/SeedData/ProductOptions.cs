
using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.SeedData;

public static class ProductOptions
{
    public static readonly Option FitType = new Option
    {
        Name = "FitType"
    };

    public static readonly Option FixingTo = new Option
    {
        Name = "FixingTo"
    };

    public static readonly Option ProductType = new Option
    {
        Name = "ProductType"
    };

    public static readonly Option Fabric = new Option
    {
        Name = "Fabric"
    };

    public static readonly Option OperationType = new Option
    {
        Name = "OperationType"
    };

    public static readonly Option OperationSide = new Option
    {
        Name = "OperationSide"
    };

    public static readonly Option HeadRailColour = new Option
    {
        Name = "HeadRailColour"
    };

    public static readonly Option SideChannelColour = new Option
    {
        Name = "SideChannelColour"
    };

    public static readonly Option RollType = new Option
    {
        Name = "RollType"
    };

    public static readonly Option ChainColour = new Option
    {
        Name = "ChainColour"
    };

    public static readonly Option ChainLength = new Option
    {
        Name = "ChainLength"
    };

    public static readonly Option BracketType = new Option
    {
        Name = "BracketType"
    };

    public static readonly Option BracketColour = new Option
    {
        Name = "BracketColour"
    };

    public static readonly Option BottomRailType = new Option
    {
        Name = "BottomRailType"
    };

    public static readonly Option BottomRailColour = new Option
    {
        Name = "BottomRailColour"
    };

    public static readonly Option PelmetType = new Option
    {
        Name = "PelmetType"
    };

    public static readonly Option PelmetColour = new Option
    {
        Name = "PelmetColour"
    };

    public static List<Option> ProductOptionsList = [
        FitType, FixingTo, ProductType,
        Fabric, OperationType, OperationSide,
        HeadRailColour, SideChannelColour, RollType,
        ChainColour, ChainLength, BracketType, BracketColour,
        BottomRailType, BottomRailColour, PelmetType,
        PelmetColour
    ];




}