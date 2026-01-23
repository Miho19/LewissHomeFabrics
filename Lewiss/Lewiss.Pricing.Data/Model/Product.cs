
namespace Lewiss.Pricing.Data.Model;
public class Product
{
    public required Guid Id {get; set;}

    public decimal Price {get; set;} = 0M;

    public required string Location {get; set;}

    public required int Width {get; set;}
    public required int Height {get; set;}

    public required Guid WorksheetId {get; set;}
    public required Worksheet Worksheet {get; set;}

    public ICollection<OptionVariation> OptionVariations {get; set;} = new List<OptionVariation>();
}