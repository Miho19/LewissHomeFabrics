namespace Lewiss.Pricing.Shared.FabricDTO;


public interface IFabricOutputDTO
{
    string FabricName { get; }
    string Opacity { get; set; }
    string Colour { get; set; }
    decimal Multiplier { get; set; }
}