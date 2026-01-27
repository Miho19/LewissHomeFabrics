using System.ComponentModel.DataAnnotations;

namespace Lewiss.Pricing.Data.Model;

public class Option
{
    public int Id;
    public required string Name { get; set; }
    public ICollection<OptionVariation> OptionVariation { get; set; } = new List<OptionVariation>();

}