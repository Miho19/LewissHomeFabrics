using Lewiss.Pricing.Shared.Product;

namespace Lewiss.Pricing.Api.Tests.Fixtures;

public static class ProductFixture
{
    public static ProductCreateDTO TestProductCreateDTO = new ProductCreateDTO
    {
        WorksheetId = WorksheetFixture.TestWorksheet.Id,
        GeneralProductConfigration = GeneralProductConfigration,

    };

    private static readonly GeneralProductConfigration GeneralProductConfigration = new GeneralProductConfigration
    {

    };
}