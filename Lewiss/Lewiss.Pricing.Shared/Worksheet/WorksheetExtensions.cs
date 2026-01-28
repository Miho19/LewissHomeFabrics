namespace Lewiss.Pricing.Shared.Worksheet;

public static class WorksheetExtensions
{
    public static WorksheetDTO ToWorksheetDTO(this Data.Model.Worksheet worksheet, Guid externalCustomerId)
    {
        if (worksheet is null)
        {
            throw new Exception("Worksheet is null");
        }
        return new WorksheetDTO
        {
            Id = worksheet.ExternalMapping,
            CustomerId = externalCustomerId,
            CallOutFee = worksheet.CallOutFee,
            Discount = worksheet.Discount,
            NewBuild = worksheet.NewBuild,
            Price = worksheet.Price
        };
    }
}