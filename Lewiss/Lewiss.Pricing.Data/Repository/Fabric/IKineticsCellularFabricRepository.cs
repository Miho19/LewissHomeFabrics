using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.Repository.Generic;

public interface IKineticsCellularFabricRepository : IRepository<KineticsCellularFabric>
{
    Task<KineticsCellularFabric?> GetFabricAsync(string colour, string opacity, CancellationToken cancellationToken);
}