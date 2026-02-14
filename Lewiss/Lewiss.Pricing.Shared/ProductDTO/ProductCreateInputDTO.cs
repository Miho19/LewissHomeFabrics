namespace Lewiss.Pricing.Shared.ProductDTO;


public class ProductCreateInputDTO
{
    public required Guid WorksheetId { get; set; }

    public required string Location { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    public required int Reveal { get; set; }
    public required int RemoteNumber { get; set; }
    public required int RemoteChannel { get; set; }
    public required int InstallHeight { get; set; }
    public required string FitType { get; set; }
    public required string FixingTo { get; set; }
    public required string ProductType { get; set; }
    public required string Fabric { get; set; }
    public required string OperationType { get; set; }
    public required string OperationSide { get; set; }

    public KineticsCellular? KineticsCellular { get; set; }
    public KineticsRoller? KineticsRoller { get; set; }
}