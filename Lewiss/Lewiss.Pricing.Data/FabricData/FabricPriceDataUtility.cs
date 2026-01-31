using Lewiss.Pricing.Data.Model.Fabric.Price;

namespace Lewiss.Pricing.Data.FabricData;

public record struct JSONPricingDataStructure()
{
    public required List<int> Width { get; init; }
    public required List<int> Height { get; init; }
    public required List<List<int>> Data { get; init; }
    public required string ProductType { get; init; }
    public required string Opacity { get; init; }

}

public record struct JSONPricingDataFileMeta
{
    public string FileName { get; init; }
    public string ProductType { get; init; }
    public string Opacity { get; init; }

};

public static class FabricPriceDataUtility
{

    private static bool IsPricingDataValid(JSONPricingDataStructure pricingData)
    {
        if (pricingData.Width is null || pricingData.Width.Count == 0)
        {
            throw new Exception("Width List is empty");
        }

        if (pricingData.Height is null || pricingData.Height.Count == 0)
        {
            throw new Exception("Height List is empty");
        }

        if (pricingData.Data is null || pricingData.Data.Count == 0)
        {
            throw new Exception("Data List is empty");
        }

        return true;
    }

    public static List<FabricPrice> PricingDataStructureToFabricPrice(JSONPricingDataStructure pricingData, string productType, string opacity)
    {
        if (!IsPricingDataValid(pricingData)) return [];


        List<FabricPrice> fabricPrices = [];

        for (int row = 0; row < pricingData.Height.Count; row++)
        {
            var height = pricingData.Height[row];
            var dataRow = pricingData.Data[row];

            for (int col = 0; col < dataRow.Count && col < pricingData.Width.Count; col++)
            {
                var fabricPrice = new FabricPrice
                {
                    Width = pricingData.Width[col],
                    Height = height,
                    Price = dataRow[col],
                    ProductType = productType,
                    Opacity = opacity
                };

                fabricPrices.Add(fabricPrice);
            }

        }
        return fabricPrices;
    }

}