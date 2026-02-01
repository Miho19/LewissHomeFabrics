namespace Lewiss.Pricing.Shared.ProductDTO;


public class ProductCreateInputDTO
{
    public required Guid WorksheetId { get; set; }
    public required FixedConfiguration FixedConfiguration { get; set; }
    public required VariableConfiguration VariableConfiguration { get; set; }
    public KineticsCellular? KineticsCellular { get; set; }
    public KineticsRoller? KineticsRoller { get; set; }
}