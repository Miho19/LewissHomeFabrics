using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class FabricOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "Fabric"
    };



    public static readonly List<ProductOptionVariation> ProductOptionVariations = [];
}