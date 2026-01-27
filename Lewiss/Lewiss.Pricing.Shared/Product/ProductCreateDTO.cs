namespace Lewiss.Pricing.Shared.Product;

public class ProductCreateDTO
{
    public required Guid WorksheetId { get; set; }
    public required GeneralProductConfigration GeneralProductConfigration { get; set; }

    public required VariationProductConfiguration VariationProductConfiguration { get; set; }
    public required ISpecificConfiguration Configuration { get; set; }
}