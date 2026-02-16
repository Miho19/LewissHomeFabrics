using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.ProductStrategy;

namespace Lewiss.Pricing.Api.Tests.Fixtures;

public static class FabricFixture
{
    public readonly static KineticsCellularFabric Cotton001Translucent = new KineticsCellularFabric
    {
        Code = "001",
        Colour = "Cotton",
        Opacity = "Translucent",
        Multiplier = 1.00m,
        ProductOptionVariationId = 1,
    };

    public readonly static KineticsCellularFabric Cream005Translucent = new KineticsCellularFabric
    {
        Code = "005",
        Colour = "Cream",
        Opacity = "Translucent",
        Multiplier = 1.00m,
        ProductOptionVariationId = 2,
    };

    public static List<FabricOutputDTO> GetFabricListKineticsCellular()
    {
        return [Cotton001Translucent.ToFabricOutputDTO(), Cream005Translucent.ToFabricOutputDTO()];
    }

    public readonly static KineticsRollerFabric AdagioBlack = new KineticsRollerFabric
    {
        Fabric = "Adagio",
        Colour = "Black",
        Opacity = "LF",
        Multiplier = 1.25m,
        MaxWidth = 3100,
        MaxHeight = 2010,
        ProductOptionVariationId = 3,
    };

    public readonly static KineticsRollerFabric Fenescreen10Charcoal = new KineticsRollerFabric
    {
        Fabric = "Fenescreen 10%",
        Colour = "Charcoal",
        Opacity = "SS",
        Multiplier = 0.9m,
        MaxWidth = 3000,
        MaxHeight = 2200,
        ProductOptionVariationId = 4,
    };

    public static List<FabricOutputDTO> GetFabricListKineticsRoller()
    {
        return [AdagioBlack.ToToFabricOutputDTO(), Fenescreen10Charcoal.ToToFabricOutputDTO()];
    }

}