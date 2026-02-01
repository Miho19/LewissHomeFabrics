namespace Lewiss.Pricing.Shared.WorksheetDTO;

public record WorksheetOutputDTO
{
    public required Guid Id { get; set; }
    public required Guid CustomerId { get; set; }
    public required decimal Price { get; set; }
    public required decimal Discount { get; set; }
    public required bool NewBuild { get; set; }
    public required decimal CallOutFee { get; set; }

}