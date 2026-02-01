namespace Lewiss.Pricing.Shared.FabricDTO;

public class KineticsRollerFabricOutputDTO : IFabricOutputDTO
{
    public string FabricName => $"{Fabric} {Colour} {Opacity}";
    public required string Opacity { get; set; }
    public required string Colour { get; set; }
    public required decimal Multiplier { get; set; }
    public required string Fabric { get; set; }

    public required int MaxWidth { get; set; }
    public required int MaxHeight { get; set; }
}