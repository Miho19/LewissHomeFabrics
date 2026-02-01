
using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lewiss.Pricing.Data.Repository.Fabric;

public class KineticsRollerFabricRepository : Repository<KineticsRollerFabric>, IKineticsRollerFabricRepository
{
    public KineticsRollerFabricRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<KineticsRollerFabric?> GetFabricAsync(string fabric, string colour, string opacity, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(f => f.Colour == colour && f.Fabric == fabric && f.Opacity == opacity, cancellationToken);

    }

    public async Task<KineticsRollerFabric?> GetFabricByProductOptionVariationIdAsync(int productOptionVariationId, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(f => f.ProductOptionVariationId == productOptionVariationId, cancellationToken);
    }
}