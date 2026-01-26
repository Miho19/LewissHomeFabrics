namespace Lewiss.Pricing.Shared.Product;

public class ProductDTO
{
    public required Guid Id { get; set; }
    public required Guid WorksheetId { get; set; }

    public required decimal Price { get; set; }
    public required string Location { get; set; }

    public required int Width { get; set; }
    public required int Height { get; set; }
    public required int Reveal { get; set; }
    public required string FitType { get; set; }
    public required string FixingTo { get; set; }
    public required bool AboveHeightConstraint { get; set; }
    public required string ProductType { get; set; }
    public required string Fabric { get; set; }
    public required string OperationType { get; set; }
    public required string OperationSide { get; set; }
    public required int RemoteNumber { get; set; }
    public required int RemoteChannel { get; set; }
    public int? ButtingToId { get; set; }

    public required IConfiguration Configuration { get; set; }

}