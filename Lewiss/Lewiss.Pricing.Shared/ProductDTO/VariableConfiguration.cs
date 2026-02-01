namespace Lewiss.Pricing.Shared.ProductDTO;


public class VariableConfiguration
{
    public required string Location { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    public required int Reveal { get; set; }
    public required int RemoteNumber { get; set; }
    public required int RemoteChannel { get; set; }
    public required decimal InstallHeight { get; set; }

}
