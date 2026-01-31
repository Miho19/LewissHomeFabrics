namespace Lewiss.Pricing.Data.Model.Fabric.Price;

public class FabricPrice
{
    public int? FabricPriceId { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    public required decimal Price { get; set; }
    public required string ProductType { get; set; }
    public required string Opacity { get; set; }

}