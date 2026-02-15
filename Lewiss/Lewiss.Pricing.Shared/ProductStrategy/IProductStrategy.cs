using FluentResults;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.QueryParameters;

namespace Lewiss.Pricing.Shared.ProductStrategy;




public interface IProductStrategy
{
    string ProductType { get; }

    Task<Result<Product>> CreateProductAsync(Guid externalCustomerId, ProductCreateInputDTO productCreateDTO, Worksheet worksheet, CancellationToken cancellationToken = default);

    Task<Result<List<FabricOutputDTO>>> GetFabricListAsync(CancellationToken cancellationToken);

    Task<Result<FabricOutputDTO>> GetFabricAsync(GetFabricQueryParameters getFabricQueryParameters, CancellationToken cancellationToken);

    // Task<Result<FabricPrice>> GetFabricPriceAsync(GetFabricQueryParameters getFabricQueryParameters, CancellationToken cancellationToken);



}