namespace Lewiss.Pricing.Shared.Fabric;


public interface IFabricDTO
{
    string FabricName { get; }
    string Opacity { get; set; }
    string Colour { get; set; }
    decimal Multiplier { get; set; }
}