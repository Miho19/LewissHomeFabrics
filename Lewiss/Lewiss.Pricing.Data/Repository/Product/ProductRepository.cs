using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lewiss.Pricing.Data.Repository.ProductRepository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Product?> GetProductByExternalIdAsync(Guid externalProductId, CancellationToken cancellationToken)
    {
        var product = await _dbSet.Include(p => p.OptionVariations)
        .ThenInclude(po => po.ProductOption)
        .FirstOrDefaultAsync(p => p.ExternalMapping == externalProductId, cancellationToken);
        return product;
    }
}