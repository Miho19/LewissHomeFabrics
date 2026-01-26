using Lewiss.Pricing.Shared.Worksheet;

namespace Lewiss.Pricing.Api.Tests.Fixtures;
public static class WorksheetFixture
{
    public static WorksheetDTO TestWorksheet = new WorksheetDTO
    {
        Id = Guid.CreateVersion7(DateTimeOffset.UtcNow),
        CustomerId = CustomerFixture.TestCustomer.Id,
        CallOutFee = 0.00m,
        Price = 0.00m,
        NewBuild = false,
        Discount = 0.00m,
    };
}