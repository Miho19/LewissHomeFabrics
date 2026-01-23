using System.ComponentModel.DataAnnotations;

namespace Lewiss.Pricing.Data.Model;
public class Worksheet
{
    public required Guid Id {get; set;}
    public required DateTimeOffset CreatedAt {get; set;}

    public required Guid CustomerId {get; set;}
    public required Customer Customer {get; set;}
    
}