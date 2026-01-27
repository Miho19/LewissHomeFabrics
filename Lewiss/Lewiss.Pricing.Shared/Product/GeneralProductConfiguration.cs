namespace Lewiss.Pricing.Shared.Product;

public class GeneralProductConfigration
{
    public required decimal Price { get; set; }
    public required string Location { get; set; }

    public required int Width { get; set; }
    public required int Height { get; set; }
    public required int Reveal { get; set; }
    public required bool AboveHeightConstraint { get; set; }
    public required int RemoteNumber { get; set; }
    public required int RemoteChannel { get; set; }

}
