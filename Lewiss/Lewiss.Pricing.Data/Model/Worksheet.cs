using System.ComponentModel.DataAnnotations;

namespace Lewiss.Pricing.Data.Model;
public class Worksheet
{
    public int Id {get; set;}
    public Guid ExternalId;
    public required DateTimeOffset CreatedAt {get; set;}

    public required int CustomerId {get; set;}
    public required Customer Customer {get; set;}
    
}