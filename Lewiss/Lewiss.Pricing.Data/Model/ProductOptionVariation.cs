
namespace Lewiss.Pricing.Data.Model;

public class ProductOptionVariation
{

    public int ProductOptionVariationId { get; set; }

    public decimal? Price { get; set; }
    public int ProductOptionId { get; set; }
    public ProductOption? ProductOption { get; set; } // delete on cascade

    public required string Value { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}