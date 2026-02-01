using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Shared.Fabric;

namespace Lewiss.Pricing.Api.Tests.Fixtures;

public static class FabricFixture
{
    public static KineticsCellularFabric Cotton001Translucent = new KineticsCellularFabric
    {
        Code = "001",
        Colour = "Cotton",
        Opacity = "Translucent",
        Multiplier = 1.00m,
        ProductOptionVariationId = 1,
    };

    public static KineticsCellularFabric Cream005Translucent = new KineticsCellularFabric
    {
        Code = "005",
        Colour = "Cream",
        Opacity = "Translucent",
        Multiplier = 1.00m,
        ProductOptionVariationId = 2,
    };

    public static List<IFabricDTO> GetFabricListKineticsCellular()
    {
        return [Cotton001Translucent.ToKineticsCellularFabricDTO(), Cream005Translucent.ToKineticsCellularFabricDTO()];
    }

    public static KineticsRollerFabric AdagioBlack = new KineticsRollerFabric
    {
        Fabric = "Adagio",
        Colour = "Black",
        Opacity = "LF",
        Multiplier = 1.25m,
        MaxWidth = 3100,
        MaxHeight = 2010,
        ProductOptionVariationId = 3,
    };

    public static KineticsRollerFabric Fenescreen10Charcoal = new KineticsRollerFabric
    {
        Fabric = "Fenescreen 10%",
        Colour = "Charcoal",
        Opacity = "SS",
        Multiplier = 0.9m,
        MaxWidth = 3000,
        MaxHeight = 2200,
        ProductOptionVariationId = 4,
    };

    public static List<IFabricDTO> GetFabricListKineticsRoller()
    {
        return [AdagioBlack.ToKineticsRollerFabricDTO(), Fenescreen10Charcoal.ToKineticsRollerFabricDTO()];
    }

}