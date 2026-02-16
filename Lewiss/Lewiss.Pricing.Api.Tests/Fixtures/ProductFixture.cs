using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.ProductDTO;

namespace Lewiss.Pricing.Api.Tests.Fixtures;

public static class ProductFixture
{

    private static readonly KineticsCellular TestKineticsCellularConfiguration = new KineticsCellular
    {
        HeadrailColour = "Black",
        SideChannelColour = "None",
    };
    public static readonly ProductCreateInputDTO TestProductCreateInputDTOKineticsCellular = new ProductCreateInputDTO()
    {
        WorksheetId = WorksheetFixture.TestWorksheet.ExternalMapping,
        Location = "Kitchen",
        Width = 1200,
        Height = 900,
        Reveal = 80,
        RemoteNumber = 0,
        RemoteChannel = 0,
        InstallHeight = 1200,
        FitType = "Inside",
        FixingTo = "Wood",
        ProductType = "Kinetics Cellular",
        Fabric = "Translucent White",
        OperationType = "Cord",
        OperationSide = "Left",
        KineticsCellular = TestKineticsCellularConfiguration,
    };

    // Product Option Variations missing for now
    public static readonly Product TestProductModelKineticsCellular = new Product()
    {
        ProductId = 1,
        ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
        Price = 1200.00m,
        Location = TestProductCreateInputDTOKineticsCellular.Location,
        Width = TestProductCreateInputDTOKineticsCellular.Width,
        Height = TestProductCreateInputDTOKineticsCellular.Height,
        Reveal = TestProductCreateInputDTOKineticsCellular.Reveal,
        RemoteNumber = TestProductCreateInputDTOKineticsCellular.RemoteNumber,
        RemoteChannel = TestProductCreateInputDTOKineticsCellular.RemoteChannel,
        InstallHeight = TestProductCreateInputDTOKineticsCellular.InstallHeight,
        WorksheetId = WorksheetFixture.TestWorksheet.WorksheetId,
        Worksheet = WorksheetFixture.TestWorksheet,
    };


    public static readonly ProductEntryOutputDTO TestProductEntryDTOKineticsCellular = new ProductEntryOutputDTO
    {
        Id = TestProductModelKineticsCellular.ExternalMapping,
        WorksheetId = TestProductModelKineticsCellular.Worksheet.ExternalMapping,
        Price = TestProductModelKineticsCellular.Price,
        Location = TestProductCreateInputDTOKineticsCellular.Location,
        Width = TestProductCreateInputDTOKineticsCellular.Width,
        Height = TestProductCreateInputDTOKineticsCellular.Height,
        Reveal = TestProductCreateInputDTOKineticsCellular.Reveal,
        RemoteNumber = TestProductCreateInputDTOKineticsCellular.RemoteNumber,
        RemoteChannel = TestProductCreateInputDTOKineticsCellular.RemoteChannel,
        InstallHeight = TestProductCreateInputDTOKineticsCellular.InstallHeight,
        FitType = TestProductCreateInputDTOKineticsCellular.FitType,
        FixingTo = TestProductCreateInputDTOKineticsCellular.FixingTo,
        ProductType = TestProductCreateInputDTOKineticsCellular.ProductType,
        Fabric = TestProductCreateInputDTOKineticsCellular.Fabric,
        OperationType = TestProductCreateInputDTOKineticsCellular.OperationType,
        OperationSide = TestProductCreateInputDTOKineticsCellular.OperationSide,
        KineticsCellular = TestKineticsCellularConfiguration
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

    public static ProductCreateInputDTO TestProductCreateInputKineticsRoller = new ProductCreateInputDTO
    {
        WorksheetId = WorksheetFixture.TestWorksheet.ExternalMapping,
        Location = "Kitchen",
        Width = 1200,
        Height = 900,
        Reveal = 80,
        RemoteNumber = 1,
        RemoteChannel = 1,
        InstallHeight = 1200,
        FitType = "IN",
        FixingTo = "Wood",
        ProductType = "Kinetics Roller",
        Fabric = "Everyday Vinyl Collection - Polar",
        OperationType = "Lithium-ion",
        OperationSide = "Left",
        KineticsRoller = TestKineticsRollerConfiguration,
    };


    // Product Option Variations missing for now
    public static readonly Product TestProductModelKineticsRoller = new Product()
    {
        ProductId = 2,
        ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
        Price = 1600.00m,
        Location = TestProductCreateInputKineticsRoller.Location,
        Width = TestProductCreateInputKineticsRoller.Width,
        Height = TestProductCreateInputKineticsRoller.Height,
        Reveal = TestProductCreateInputKineticsRoller.Reveal,
        RemoteNumber = TestProductCreateInputKineticsRoller.RemoteNumber,
        RemoteChannel = TestProductCreateInputKineticsRoller.RemoteChannel,
        InstallHeight = TestProductCreateInputKineticsRoller.InstallHeight,
        WorksheetId = WorksheetFixture.TestWorksheet.WorksheetId,
        Worksheet = WorksheetFixture.TestWorksheet,
    };



    public static readonly ProductEntryOutputDTO TestProductEntryOutputDTOKineticsRoller = new ProductEntryOutputDTO
    {
        Id = TestProductModelKineticsRoller.ExternalMapping,
        WorksheetId = TestProductModelKineticsRoller.Worksheet.ExternalMapping,
        Price = TestProductModelKineticsRoller.Price,
        Location = TestProductCreateInputDTOKineticsCellular.Location,
        Width = TestProductModelKineticsRoller.Width,
        Height = TestProductModelKineticsRoller.Height,
        Reveal = TestProductModelKineticsRoller.Reveal,
        RemoteNumber = TestProductModelKineticsRoller.RemoteNumber,
        RemoteChannel = TestProductModelKineticsRoller.RemoteChannel,
        InstallHeight = TestProductModelKineticsRoller.InstallHeight,
        FitType = TestProductCreateInputKineticsRoller.FitType,
        FixingTo = TestProductCreateInputKineticsRoller.FixingTo,
        ProductType = TestProductCreateInputKineticsRoller.ProductType,
        Fabric = TestProductCreateInputKineticsRoller.Fabric,
        OperationType = TestProductCreateInputKineticsRoller.OperationType,
        OperationSide = TestProductCreateInputKineticsRoller.OperationSide,
        KineticsRoller = TestKineticsRollerConfiguration
    };


}