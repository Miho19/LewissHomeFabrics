namespace Lewiss.Pricing.Shared.Product;

public class ProductEntryDTO
{
    public required Guid Id { get; set; }
    public required Guid WorksheetId { get; set; }

    public required GeneralProductConfigration GeneralConfiguration { get; set; }

    public required ISpecificConfiguration Configuration { get; set; }

}