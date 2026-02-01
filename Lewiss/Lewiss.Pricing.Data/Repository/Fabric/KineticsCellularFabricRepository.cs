
using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lewiss.Pricing.Data.Repository.Fabric;

public class KineticsCellularFabricRepository : Repository<KineticsCellularFabric>, IKineticsCellularFabricRepository
{
    public KineticsCellularFabricRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<KineticsCellularFabric?> GetFabricAsync(string colour, string opacity, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(f => f.Colour == colour && f.Opacity == opacity, cancellationToken);
    }
}