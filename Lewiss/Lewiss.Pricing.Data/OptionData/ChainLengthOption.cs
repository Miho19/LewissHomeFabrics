using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Data.OptionData;

public static class ChainLengthOption
{
    public static readonly ProductOption ProductOption = new ProductOption
    {
        ProductOptionId = OptionDataUtility.GetOptionId(),
        Name = "ChainLength"
    };

    private static List<ProductOptionVariation>? GenerateChainLengths(int start, int end, int step)
    {
        if (start < 0) return null;
        if (start >= end) return null;
        if (step >= end) return null;

        var stepList = new List<int>();

        for (int i = start; i <= end; i += step)
            stepList.Add(i);

        var chainLengths = stepList.Select(s => new ProductOptionVariation
        {
            ProductOptionVariationId = OptionDataUtility.GetOptionVariationId(),
            ProductOptionId = ProductOption.ProductOptionId,
            Value = s.ToString(),
            Price = 0.00m,
        }).ToList();

        return chainLengths;
    }

    public static readonly List<ProductOptionVariation> ProductOptionVariations = [
        ..GenerateChainLengths(750, 3000, 250)!
    ];
}