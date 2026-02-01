namespace Lewiss.Pricing.Shared.ProductDTO;

public class FixedConfiguration
{
    public required string FitType { get; set; }
    public required string FixingTo { get; set; }
    public required string ProductType { get; set; }
    public required string Fabric { get; set; }
    public required string OperationType { get; set; }
    public required string OperationSide { get; set; }
}