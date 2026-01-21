using Lewiss.Pricing.Shared.Customer;

namespace Lewiss.Pricing.Shared.Worksheet;

public record WorksheetDTO
{
    public required Guid WorksheetId {get; set;}
    public required CustomerDTO Customer {get; set;}

    public required DateTimeOffset CreatedAt {get; set;}

}