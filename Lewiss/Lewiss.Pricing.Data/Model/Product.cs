using System.ComponentModel.DataAnnotations;

namespace Lewiss.Pricing.Data.Model;
public class Product
{
    [Key]
    public required Guid Id {get; set;}

    public decimal Price {get; set;} = 0M;
    public required Guid WorksheetId {get; set;}
    public required Worksheet Worksheet {get; set;}

    public ICollection<OptionVariation> OptionVariations {get; set;} = new List<OptionVariation>();
}