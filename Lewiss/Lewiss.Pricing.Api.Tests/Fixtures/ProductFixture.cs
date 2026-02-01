using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.ProductDTO;

namespace Lewiss.Pricing.Api.Tests.Fixtures;

public static class ProductFixture
{

    /// <summary>
    /// Kinetics Cellular
    /// </summary>
    private static readonly FixedConfiguration TestKineticsCellularFixedConfiguration = new FixedConfiguration
    {
        FitType = "Inside",
        FixingTo = "Wood",
        ProductType = "Kinetics Cellular",
        Fabric = "Translucent White",
        OperationType = "Cord",
        OperationSide = "Left",
    };

    private static readonly VariableConfiguration TestKineticsCellularVariableConfiguration = new VariableConfiguration
    {
        Location = "Kitchen",
        Width = 1200,
        Height = 900,
        Reveal = 80,
        InstallHeight = 1.20m,
        RemoteNumber = 0,
        RemoteChannel = 0,
    };

    private static readonly KineticsCellular TestKineticsCellularConfiguration = new KineticsCellular
    {
        HeadrailColour = "Black",
        SideChannelColour = "None",
    };

    public static ProductCreateInputDTO TestProductCreateDTOKineticsCellular = new ProductCreateInputDTO
    {
        WorksheetId = WorksheetFixture.TestWorksheet.ExternalMapping,
        VariableConfiguration = TestKineticsCellularVariableConfiguration,
        FixedConfiguration = TestKineticsCellularFixedConfiguration,
        KineticsCellular = TestKineticsCellularConfiguration,
    };


    // This does not include variations
    public static Product TestProductKineticsCellular = new Product()
    {
        ProductId = 1,
        ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
        Price = 1200.00m,
        Location = TestKineticsCellularVariableConfiguration.Location,
        Width = TestKineticsCellularVariableConfiguration.Width,
        Height = TestKineticsCellularVariableConfiguration.Height,
        Reveal = TestKineticsCellularVariableConfiguration.Reveal,
        InstallHeight = TestKineticsCellularVariableConfiguration.InstallHeight,
        RemoteNumber = TestKineticsCellularVariableConfiguration.RemoteNumber,
        RemoteChannel = TestKineticsCellularVariableConfiguration.RemoteChannel,
        WorksheetId = WorksheetFixture.TestWorksheet.WorksheetId,
        Worksheet = WorksheetFixture.TestWorksheet,
    };


    public static ProductEntryOutputDTO TestProductEntryDTOKineticsCellular = new ProductEntryOutputDTO
    {
        Id = TestProductKineticsCellular.ExternalMapping,
        WorksheetId = WorksheetFixture.TestWorksheetDTO.Id,
        FixedConfiguration = TestKineticsCellularFixedConfiguration,
        VariableConfiguration = TestKineticsCellularVariableConfiguration,
        Price = TestProductKineticsCellular.Price,
        KineticsCellular = TestKineticsCellularConfiguration
    };

    /// <summary>
    /// Kinetics Roller 
    /// </summary>


    private static readonly FixedConfiguration TestKineticsRollerFixedConfiguration = new FixedConfiguration
    {
        FitType = "IN",
        FixingTo = "Wood",
        ProductType = "Kinetics Roller",
        Fabric = "Everyday Vinyl Collection - Polar",
        OperationType = "Lithium-ion",
        OperationSide = "Left",
    };



    private static readonly VariableConfiguration TestKineticsRollerVariableConfiguration = new VariableConfiguration
    {

        Location = "Kitchen",
        Width = 1200,
        Height = 900,
        Reveal = 80,
        InstallHeight = 1.20m,
        RemoteNumber = 1,
        RemoteChannel = 1,
    };



    private static readonly KineticsRoller TestKineticsRollerConfiguration = new KineticsRoller
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


    public static ProductCreateInputDTO TestProductCreateDTOKineticsRoller = new ProductCreateInputDTO
    {
        WorksheetId = WorksheetFixture.TestWorksheet.ExternalMapping,
        VariableConfiguration = TestKineticsRollerVariableConfiguration,
        FixedConfiguration = TestKineticsRollerFixedConfiguration,
        KineticsRoller = TestKineticsRollerConfiguration,
    };



}