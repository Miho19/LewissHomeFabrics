using Lewiss.Pricing.Data.Model.Fabric;
using Lewiss.Pricing.Shared.Fabric;

public static class FabricExtensions
{
    public static KineticsCellularFabricDTO ToKineticsCellularFabricDTO(this KineticsCellularFabric kineticsCellularFabric)
    {
        if (kineticsCellularFabric is null)
        {
            throw new Exception("Kinetics Cellular Fabric is null");
        }

        return new KineticsCellularFabricDTO
        {
            Opacity = kineticsCellularFabric.Opacity,
            Colour = kineticsCellularFabric.Colour,
            Code = kineticsCellularFabric.Code,
            Multiplier = kineticsCellularFabric.Multiplier,
        };
    }

    public static KineticsRollerFabricDTO ToKineticsRollerFabricDTO(this KineticsRollerFabric kineticsRollerFabric)
    {
        if (kineticsRollerFabric is null)
        {
            throw new Exception("Kinetics Roller Fabric is null");
        }

        return new KineticsRollerFabricDTO
        {
            Opacity = kineticsRollerFabric.Opacity,
            Colour = kineticsRollerFabric.Colour,
            Multiplier = kineticsRollerFabric.Multiplier,
            Fabric = kineticsRollerFabric.Fabric,
            MaxWidth = kineticsRollerFabric.MaxWidth,
            MaxHeight = kineticsRollerFabric.MaxHeight,
        };
    }
}