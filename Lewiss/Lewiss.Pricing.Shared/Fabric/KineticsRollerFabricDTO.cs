public class KineticsRollerFabricDTO : IFabricDTO
{
    public string FabricName => $"{Fabric} {Colour} {Opacity}";
    public required string Opacity { get; set; }
    public required string Colour { get; set; }
    public required decimal Multiplier { get; set; }
    public required string Fabric { get; set; }
}