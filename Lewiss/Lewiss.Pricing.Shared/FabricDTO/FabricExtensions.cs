using Lewiss.Pricing.Data.Model.Fabric.Type;


namespace Lewiss.Pricing.Shared.FabricDTO;

public static class FabricExtensions
{
    public static KineticsCellularFabricOutputDTO ToKineticsCellularFabricOutputDTO(this KineticsCellularFabric kineticsCellularFabric)
    {
        if (kineticsCellularFabric is null)
        {
            throw new Exception("Kinetics Cellular Fabric is null");
        }

        return new KineticsCellularFabricOutputDTO
        {
            Opacity = kineticsCellularFabric.Opacity,
            Colour = kineticsCellularFabric.Colour,
            Code = kineticsCellularFabric.Code,
            Multiplier = kineticsCellularFabric.Multiplier,
        };
    }

    public static KineticsRollerFabricOutputDTO ToKineticsRollerFabricOutputDTO(this KineticsRollerFabric kineticsRollerFabric)
    {
        if (kineticsRollerFabric is null)
        {
            throw new Exception("Kinetics Roller Fabric is null");
        }

        return new KineticsRollerFabricOutputDTO
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