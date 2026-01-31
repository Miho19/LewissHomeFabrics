
using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model.Fabric;
using Lewiss.Pricing.Data.Repository.Generic;

namespace Lewiss.Pricing.Data.Repository.Fabric;

class KineticsRollerFabricRepository : Repository<KineticsRollerFabric>, IKineticsRollerFabricRepository
{
    public KineticsRollerFabricRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }
}