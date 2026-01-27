
namespace Lewiss.Pricing.Data.Model;

public class OptionVariation
{

    public int Id { get; set; }

    public decimal? Price { get; set; }
    public required int OptionId { get; set; }
    public required Option Option { get; set; } // delete on cascade

    public required string Value { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}