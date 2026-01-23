using System.ComponentModel.DataAnnotations;

namespace Lewiss.Pricing.Data.Model;
public class Option
{
    public required Guid Id;
    public required string Name {get; set;}
    
}