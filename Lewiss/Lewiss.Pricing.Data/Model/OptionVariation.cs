
namespace Lewiss.Pricing.Data.Model;
public class OptionVariation
{

    public required Guid Id {get; set;}

    public decimal? Price {get; set;}
    public required Guid OptionId {get; set;}
    public required Option Option {get; set;} // delete on cascade

    public required string Value {get; set;}

    public ICollection<Product> Products {get; set;} = new List<Product>();
}