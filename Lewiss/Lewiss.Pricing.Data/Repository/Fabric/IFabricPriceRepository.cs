using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Data.Repository.Generic;

namespace Lewiss.Pricing.Data.Repository.Fabric;

public interface IFabricPriceRepository : IRepository<FabricPrice>
{
    Task<FabricPrice?> GetFabricPriceByFabricPriceQueryParametersAsync(string productType, int width, int height, string opacity, CancellationToken cancellationToken);
}