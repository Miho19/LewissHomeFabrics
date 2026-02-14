namespace Lewiss.Pricing.Shared.QueryParameters;

public class GetFabricQueryParameters
{
    // common

    public int? Width { get; set; }
    public int? Height { get; set; }

    public string? Colour { get; set; }
    public string? Opacity { get; set; }

    // Kinetics Roller Specific
    public string? Fabric { get; set; }


    public void Deconstruct(out int? width, out int? height, out string? colour, out string? opacity, out string? fabric)
    {
        width = Width;
        height = Height;
        colour = Colour;
        opacity = Opacity;
        fabric = Fabric;
    }
}