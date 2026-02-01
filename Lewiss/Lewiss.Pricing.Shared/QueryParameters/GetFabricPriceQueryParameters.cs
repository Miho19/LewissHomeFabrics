namespace Lewiss.Pricing.Shared.QueryParameters;

public class GetFabricPriceQueryParameters
{
    // common

    public required int Width { get; set; }
    public required int Height { get; set; }

    public required string Colour { get; set; }
    public required string Opacity { get; set; }

    // Kinetics Roller Specific
    public string? Fabric { get; set; }


    public void Deconstruct(out int width, out int height, out string colour, out string opacity, out string? fabric)
    {
        width = Width;
        height = Height;
        colour = Colour;
        opacity = Opacity;
        fabric = Fabric;
    }
}