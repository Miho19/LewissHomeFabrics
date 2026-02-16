using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.WorksheetDTO;

namespace Lewiss.Pricing.Api.Tests.Fixtures;

public static class WorksheetFixture
{
    public readonly static WorksheetOutputDTO TestWorksheetDTO = new WorksheetOutputDTO
    {
        Id = Guid.CreateVersion7(DateTimeOffset.UtcNow),
        CustomerId = CustomerFixture.TestCustomer.ExternalMapping,
        CallOutFee = 0.00m,
        Price = 2000.00m,
        NewBuild = false,
        Discount = 0.00m,
    };

    public readonly static Worksheet TestWorksheet = new Worksheet()
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