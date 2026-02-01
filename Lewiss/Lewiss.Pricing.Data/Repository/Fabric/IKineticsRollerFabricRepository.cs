using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.Repository.Generic;

public interface IKineticsRollerFabricRepository : IRepository<KineticsRollerFabric>
{
    Task<KineticsRollerFabric?> GetFabricAsync(string fabric, string colour, string opacity, CancellationToken cancellationToken);
    Task<KineticsRollerFabric?> GetFabricByProductOptionVariationIdAsync(int productOptionVariationId, CancellationToken cancellationToken);
}