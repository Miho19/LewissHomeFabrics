using Lewiss.Pricing.Data.Model.Fabric;

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

}