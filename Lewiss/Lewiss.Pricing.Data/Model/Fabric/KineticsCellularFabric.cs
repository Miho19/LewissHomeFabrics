namespace Lewiss.Pricing.Data.Model.Fabric;

public class KineticsCellularFabric
{
    public int KineticsCellularFabricId { get; set; }
    public required string Code { get; set; }
    public required string Colour { get; set; }
    public required string Opacity { get; set; }
    public required decimal Multiplier { get; set; }
    public required int ProductOptionVariationId { get; set; }
    public required ProductOptionVariation ProductOptionVariation { get; set; }
}