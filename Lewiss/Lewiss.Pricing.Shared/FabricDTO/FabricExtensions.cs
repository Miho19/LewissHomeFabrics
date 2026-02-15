using Lewiss.Pricing.Data.Model.Fabric.Type;


namespace Lewiss.Pricing.Shared.FabricDTO;

public static class FabricExtensions
{
    public static FabricOutputDTO ToFabricOutputDTO(this KineticsCellularFabric kineticsCellularFabric)
    {

        return new FabricOutputDTO
        {
            FabricName = $"{kineticsCellularFabric.Code} {kineticsCellularFabric.Opacity} {kineticsCellularFabric.Colour}",
            Opacity = kineticsCellularFabric.Opacity,
            Colour = kineticsCellularFabric.Colour,
            Code = kineticsCellularFabric.Code,
            Multiplier = kineticsCellularFabric.Multiplier,
        };
    }

    public static FabricOutputDTO ToToFabricOutputDTO(this KineticsRollerFabric kineticsRollerFabric)
    {

        return new FabricOutputDTO
        {
            FabricName = $"{kineticsRollerFabric.Fabric} {kineticsRollerFabric.Colour} {kineticsRollerFabric.Opacity}",
            Opacity = kineticsRollerFabric.Opacity,
            Colour = kineticsRollerFabric.Colour,
            Multiplier = kineticsRollerFabric.Multiplier,
            Fabric = kineticsRollerFabric.Fabric,
            MaxWidth = kineticsRollerFabric.MaxWidth,
            MaxHeight = kineticsRollerFabric.MaxHeight,
        };
    }

}