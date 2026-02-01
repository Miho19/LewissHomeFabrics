using System.Text.RegularExpressions;
using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Data.Repository.Fabric;
using Lewiss.Pricing.Data.Repository.Generic;
using Microsoft.EntityFrameworkCore;

public class FabricPriceRepository : Repository<FabricPrice>, IFabricPriceRepository
{
    public FabricPriceRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<FabricPrice?> GetFabricPriceByFabricPriceQueryParametersAsync(string productType, int width, int height, string opacity, CancellationToken cancellationToken)
    {
        var fabricPrice = await _dbSet
        .FirstOrDefaultAsync(fp => fp.Width == width && fp.Height == height && fp.ProductType == productType && fp.Opacity == opacity, cancellationToken);
        return fabricPrice;
    }


}