namespace Lewiss.Pricing.Data.Model.Fabric;

public class KineticsRollerFabric
{
    public int KineticsRollerFabricId { get; set; }
    public required string Fabric { get; set; }
    public required string Colour { get; set; }
    public required string Opacity { get; set; }
    public required decimal Multiplier { get; set; }
    public required int MaxWidth { get; set; }
    public required int MaxHeight { get; set; }

    public required int ProductOptionVariationId { get; set; }
    public ProductOptionVariation? ProductOptionVariation { get; set; }
}