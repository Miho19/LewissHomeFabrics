using FluentResults;
using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.QueryParameters;

namespace Lewiss.Pricing.Shared.Strategy;




public interface IProductStrategy<FabricType> where FabricType : class
{
    string ProductType { get; }
    Task<Result<List<FabricType>>> GetFabricListAsync(CancellationToken cancellationToken);

    Task<Result<FabricType>> GetFabricAsync(GetFabricQueryParameters getFabricQueryParameters, CancellationToken cancellationToken);

    Task<Result<FabricPriceOutputDTO>> GetFabricPriceAsync(GetFabricQueryParameters getFabricQueryParameters, CancellationToken cancellationToken);



}