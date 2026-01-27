namespace Lewiss.Pricing.Data.Model;

public class ProductOption
{
    public int ProductOptionId { get; set; }
    public required string Name { get; set; }
    public ICollection<ProductOptionVariation> ProductOptionVariation { get; set; } = new List<ProductOptionVariation>();

}