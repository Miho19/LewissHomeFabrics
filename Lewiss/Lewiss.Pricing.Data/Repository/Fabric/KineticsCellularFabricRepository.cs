
using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.Repository.Generic;

namespace Lewiss.Pricing.Data.Repository.Fabric;

public class KineticsCellularFabricRepository : Repository<KineticsCellularFabric>, IKineticsCellularFabricRepository
{
    public KineticsCellularFabricRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }
}