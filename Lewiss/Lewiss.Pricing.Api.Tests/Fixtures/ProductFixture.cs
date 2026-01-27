using Lewiss.Pricing.Shared.Product;

namespace Lewiss.Pricing.Api.Tests.Fixtures;

public static class ProductFixture
{

    private static readonly VariationProductConfiguration TestKineticsCellularVariationProductConfiguration = new VariationProductConfiguration
    {
        FitType = "IN",
        FixingTo = "Wood",
        ProductType = "kineticscellular",
        Fabric = "Translucent White",
        OperationType = "Cord",
        OperationSide = "Left",
    };

    private static readonly VariationProductConfiguration TestKineticsRollerVariationProductConfiguration = new VariationProductConfiguration
    {
        FitType = "IN",
        FixingTo = "Wood",
        ProductType = "kineticsroller",
        Fabric = "Everyday Vinyl Collection - Polar",
        OperationType = "Lithium-ion",
        OperationSide = "Left",
    };

    private static readonly GeneralProductConfigration TestKineticsCellularGeneralProductConfigration = new GeneralProductConfigration
    {
        Price = 0.00m,
        Location = "Kitchen",
        Width = 1200,
        Height = 900,
        Reveal = 80,
        AboveHeightConstraint = false,
        RemoteNumber = 0,
        RemoteChannel = 0,
    };

    private static readonly GeneralProductConfigration TestKineticsRollerGeneralProductConfigration = new GeneralProductConfigration
    {
        Price = 0.00m,
        Location = "Kitchen",
        Width = 1200,
        Height = 900,
        Reveal = 80,
        AboveHeightConstraint = false,
        RemoteNumber = 1,
        RemoteChannel = 1,
    };

    private static readonly ISpecificConfiguration TestKineticsCellularConfiguration = new KineticsCellularDTO
    {
        HeadRailColour = "Black",
        SideChannelColour = "None",
    };

    private static readonly ISpecificConfiguration TestKineticsRollerConfiguration = new KineticsRollerDTO
    {
        RollType = "Front",
        ChainColour = null,
        ChainLength = null,
        BracketType = "Standard",
        BracketColour = "White",
        BottomRailType = "Flat",
        BottomRailColour = "Anodised",
        PelmetType = null,
        PelmetColour = null,
    };
    public static ProductCreateDTO TestProductCreateDTOKineticsCellular = new ProductCreateDTO
    {
        WorksheetId = WorksheetFixture.TestWorksheet.Id,
        GeneralProductConfigration = TestKineticsCellularGeneralProductConfigration,
        VariationProductConfiguration = TestKineticsCellularVariationProductConfiguration,
        Configuration = TestKineticsCellularConfiguration,

    };

    public static ProductCreateDTO TestProductCreateDTOKineticsRoller = new ProductCreateDTO
    {
        WorksheetId = WorksheetFixture.TestWorksheet.Id,
        GeneralProductConfigration = TestKineticsRollerGeneralProductConfigration,
        VariationProductConfiguration = TestKineticsRollerVariationProductConfiguration,
        Configuration = TestKineticsRollerConfiguration,

    };

}