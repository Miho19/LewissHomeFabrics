namespace Lewiss.Pricing.Shared.Product;

public class ProductCreateDTO
{
    public required Guid WorksheetId { get; set; }
    public required FixedConfiguration FixedConfiguration { get; set; }
    public required VariableConfiguration VariableConfiguration { get; set; }
    public KineticsCellular? KineticsCellular { get; set; }
    public KineticsRoller? KineticsRoller { get; set; }
}