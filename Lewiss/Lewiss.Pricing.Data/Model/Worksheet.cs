
using System.ComponentModel.DataAnnotations;

namespace Lewiss.Pricing.Data.Model;

public class Worksheet
{
    [Key]
    public int WorksheetId { get; set; }
    public required Guid ExternalMapping { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }

    public required decimal Price { get; set; }
    public required decimal Discount { get; set; }
    public required bool NewBuild { get; set; }
    public required decimal CallOutFee { get; set; }
    public required int CustomerId { get; set; }
    public required Customer Customer { get; set; }

}