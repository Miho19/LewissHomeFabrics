
using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model.Fabric;
using Lewiss.Pricing.Data.Repository.Generic;

namespace Lewiss.Pricing.Data.Repository.Fabric;

class KineticsCellularFabricRepository : Repository<KineticsCellularFabric>, IKineticsCellularFabricRepository
{
    public KineticsCellularFabricRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }
}