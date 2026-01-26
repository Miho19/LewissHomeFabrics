using Lewiss.Pricing.Shared.Product;

public class KineticsRollerDTO : IConfiguration
{
    public string ProductType { get; } = "KineticsRoller";
    public required string RollType { get; set; }
    public required string ChainColour { get; set; }
    public required int ChainLength { get; set; }
    public required string BracketType { get; set; }
    public required string BracketColour { get; set; }
    public required string BottomRailType { get; set; }
    public required string BottomRailColour { get; set; }
    public required string PelmetType { get; set; }
    public required string PelmetColour { get; set; }
}