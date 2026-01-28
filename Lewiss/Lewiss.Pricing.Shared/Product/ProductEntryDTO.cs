namespace Lewiss.Pricing.Shared.Product;

public class ProductEntryDTO
{
    public required Guid Id { get; set; }
    public required Guid WorksheetId { get; set; }

    public required decimal Price { get; set; } = 0.00m;

    public required VariableConfiguration VariableConfiguration { get; set; }

    public required FixedConfiguration FixedConfiguration { get; set; }

    public KineticsCellular? KineticsCellular { get; set; }
    public KineticsRoller? KineticsRoller { get; set; }

}