using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.Product;

namespace Lewiss.Pricing.Api.Tests.Fixtures;

public static class ProductFixture
{

    private static readonly VariationProductConfiguration TestKineticsCellularVariationProductConfiguration = new VariationProductConfiguration
    {
        FitType = "Inside",
        FixingTo = "Wood",
        ProductType = "Kinetics Cellular",
        Fabric = "Translucent White",
        OperationType = "Cord",
        OperationSide = "Left",
    };

    private static readonly GeneralProductConfigration TestKineticsCellularGeneralProductConfigration = new GeneralProductConfigration
    {
        Price = 1200.00m,
        Location = "Kitchen",
        Width = 1200,
        Height = 900,
        Reveal = 80,
        AboveHeightConstraint = false,
        RemoteNumber = 0,
        RemoteChannel = 0,
    };

    private static readonly ISpecificConfiguration TestKineticsCellularConfiguration = new KineticsCellularDTO
    {
        HeadRailColour = "Black",
        SideChannelColour = "None",
    };

    public static ProductCreateDTO TestProductCreateDTOKineticsCellular = new ProductCreateDTO
    {
        WorksheetId = WorksheetFixture.TestWorksheet.ExternalMapping,
        GeneralProductConfigration = TestKineticsCellularGeneralProductConfigration,
        VariationProductConfiguration = TestKineticsCellularVariationProductConfiguration,
        Configuration = TestKineticsCellularConfiguration,

    };


    public static Product TestProductKineticsCellular = new Product()
    {
        ProductId = 1,
        ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
        Price = TestKineticsCellularGeneralProductConfigration.Price,
        Location = TestKineticsCellularGeneralProductConfigration.Location,
        Width = TestKineticsCellularGeneralProductConfigration.Width,
        Height = TestKineticsCellularGeneralProductConfigration.Height,
        Reveal = TestKineticsCellularGeneralProductConfigration.Reveal,
        AboveHeightConstraint = TestKineticsCellularGeneralProductConfigration.AboveHeightConstraint,
        RemoteNumber = TestKineticsCellularGeneralProductConfigration.RemoteNumber,
        RemoteChannel = TestKineticsCellularGeneralProductConfigration.RemoteChannel,
        WorksheetId = WorksheetFixture.TestWorksheet.WorksheetId,
        Worksheet = WorksheetFixture.TestWorksheet,
    };




    private static readonly VariationProductConfiguration TestKineticsRollerVariationProductConfiguration = new VariationProductConfiguration
    {
        FitType = "IN",
        FixingTo = "Wood",
        ProductType = "Kinetics Roller",
        Fabric = "Everyday Vinyl Collection - Polar",
        OperationType = "Lithium-ion",
        OperationSide = "Left",
    };



    private static readonly GeneralProductConfigration TestKineticsRollerGeneralProductConfigration = new GeneralProductConfigration
    {
        Price = 800.00m,
        Location = "Kitchen",
        Width = 1200,
        Height = 900,
        Reveal = 80,
        AboveHeightConstraint = false,
        RemoteNumber = 1,
        RemoteChannel = 1,
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


    public static ProductCreateDTO TestProductCreateDTOKineticsRoller = new ProductCreateDTO
    {
        WorksheetId = WorksheetFixture.TestWorksheet.ExternalMapping,
        GeneralProductConfigration = TestKineticsRollerGeneralProductConfigration,
        VariationProductConfiguration = TestKineticsRollerVariationProductConfiguration,
        Configuration = TestKineticsRollerConfiguration,

    };



}