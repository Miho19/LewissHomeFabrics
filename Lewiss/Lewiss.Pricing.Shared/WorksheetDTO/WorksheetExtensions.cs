namespace Lewiss.Pricing.Shared.WorksheetDTO;

public static class WorksheetExtensions
{
    public static WorksheetOutputDTO ToWorksheetDTO(this Data.Model.Worksheet worksheet, Guid externalCustomerId)
    {
        if (worksheet is null)
        {
            throw new Exception("Worksheet is null");
        }
        return new WorksheetOutputDTO
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