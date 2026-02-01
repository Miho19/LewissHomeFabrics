namespace Lewiss.Pricing.Shared.ProductDTO;


public class KineticsRoller
{
    public required string RollType { get; set; }
    public string? ChainColour { get; set; }
    public string? ChainLength { get; set; }
    public required string BracketType { get; set; }
    public required string BracketColour { get; set; }
    public required string BottomRailType { get; set; }
    public required string BottomRailColour { get; set; }
    public string? PelmetType { get; set; }
    public string? PelmetColour { get; set; }
}