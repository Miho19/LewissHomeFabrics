namespace Lewiss.Pricing.Shared.FabricDTO;

public class KineticsCellularFabricOutputDTO : IFabricOutputDTO
{
    public required string Opacity { get; set; }
    public required string Colour { get; set; }
    public required decimal Multiplier { get; set; }
    public required string Code { get; set; }
    public string FabricName => $"{Code} {Opacity} {Colour}";

}