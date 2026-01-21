using System.ComponentModel.DataAnnotations;

namespace Lewiss.Pricing.Data.Model;
public class Product
{
    [Key]
    public required Guid Id {get; set;}

    public required Guid WorksheetId {get; set;}
    public required Worksheet Worksheet {get; set;}
}