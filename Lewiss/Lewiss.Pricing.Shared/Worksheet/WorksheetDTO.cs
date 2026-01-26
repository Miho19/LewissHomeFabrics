using Lewiss.Pricing.Shared.CustomerDTO;

namespace Lewiss.Pricing.Shared.Worksheet;

public record WorksheetDTO
{
    public required Guid Id {get; set;}
    public required Guid CustomerId {get; set;}
    public required decimal Price {get; set;}
    public required decimal Additional {get; set;}

}