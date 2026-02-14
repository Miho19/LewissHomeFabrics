using FluentResults;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.ProductDTO;

namespace Lewiss.Pricing.Shared.ProductStrategy;


public class KineticsRollerProduct : IProductStrategy<KineticsRollerFabric>
{
    public string ProductType => ProductTypeOption.KineticsRoller.Value;

    public Task<Result<Product>> CreateProductAsync(Guid externalCustomerId, Guid externalWorksheetId, ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}