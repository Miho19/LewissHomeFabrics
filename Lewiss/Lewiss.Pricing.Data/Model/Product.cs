
namespace Lewiss.Pricing.Data.Model;

public class Product
{
    public int Id { get; set; }

    public required Guid ExternalMapping { get; set; }

    public decimal Price { get; set; } = 0M;

    public required string Location { get; set; }

    public required int Width { get; set; }
    public required int Height { get; set; }

    public required int Reveal { get; set; }

    public required bool AboveHeightConstraint { get; set; }

    public required int RemoteNumber { get; set; }
    public required int RemoteChannel { get; set; }

    public required int WorksheetId { get; set; }
    public required Worksheet Worksheet { get; set; }

    public ICollection<OptionVariation> OptionVariations { get; set; } = new List<OptionVariation>();
}