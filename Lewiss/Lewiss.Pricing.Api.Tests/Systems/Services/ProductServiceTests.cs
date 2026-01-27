using Lewiss.Pricing.Api.Tests.Fixtures;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.Services;
using Moq;
using Xunit.Abstractions;

namespace Lewiss.Pricing.Api.Tests.Systems.Services;

public class ProductServiceTests
{
    private readonly ITestOutputHelper _logger;
    public ProductServiceTests(ITestOutputHelper logger)
    {
        _logger = logger;
    }


    [Fact(Skip = "TODO SOON")]
    public async Task PopulateProductOptionVariation_ShouldReturnValidProduct_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productService = new ProductService(unitOfWorkMock.Object);

        var result = await productService.PopulateProductOptionVariationList(ProductFixture.TestProductKineticsCellular, ProductFixture.TestProductCreateDTOKineticsCellular);
        Assert.NotNull(result);
        Assert.IsType<Product>(result);
        Assert.NotEmpty(result.OptionVariations);
    }
}