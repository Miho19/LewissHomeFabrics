namespace Lewiss.Pricing.Shared.FabricDTO;

public class FabricOutputDTO
{
    public required string FabricName { get; set; }
    public required string Opacity { get; set; }
    public required string Colour { get; set; }
    public required decimal Multiplier { get; set; }

    public string? Code { get; set; }
    public string? Fabric { get; set; }

    public int MaxWidth { get; set; }
    public int MaxHeight { get; set; }

}

// cellular public string FabricName => $"{Code} {Opacity} {Colour}";
// roller public string FabricName => $"{Fabric} {Colour} {Opacity}";