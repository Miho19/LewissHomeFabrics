using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.Worksheet;

namespace Lewiss.Pricing.Api.Tests.Fixtures;

public static class WorksheetFixture
{
    public static WorksheetDTO TestWorksheetDTO = new WorksheetDTO
    {
        Id = Guid.CreateVersion7(DateTimeOffset.UtcNow),
        CustomerId = CustomerFixture.TestCustomer.ExternalMapping,
        CallOutFee = 0.00m,
        Price = 2000.00m,
        NewBuild = false,
        Discount = 0.00m,
    };

    public static Worksheet TestWorksheet = new Worksheet()
    {
        WorksheetId = 1,
        ExternalMapping = TestWorksheetDTO.Id,
        CustomerId = CustomerFixture.TestCustomer.CustomerId,
        Customer = CustomerFixture.TestCustomer,
        Price = TestWorksheetDTO.Price,
        Discount = TestWorksheetDTO.Discount,
        NewBuild = TestWorksheetDTO.NewBuild,
        CallOutFee = TestWorksheetDTO.CallOutFee,
        CreatedAt = DateTimeOffset.UtcNow,
    };
}