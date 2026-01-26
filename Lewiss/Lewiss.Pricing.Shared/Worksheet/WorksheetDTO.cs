using Lewiss.Pricing.Shared.Customer;

namespace Lewiss.Pricing.Shared.Worksheet;

public record WorksheetDTO
{
    public required Guid Id {get; set;}
    public required CustomerDTO Customer {get; set;}
    public required decimal Price {get; set;}
    public required decimal Additional {get; set;}

}