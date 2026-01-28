using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lewiss.Pricing.Data.Repository.ProductOptionRepository;

public class ProductOptionRepository : Repository<ProductOption>, IProductOptionRepository
{
    public ProductOptionRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ProductOption?> GetProductOptionByNameAsync(string productOptionName, CancellationToken cancellationToken)
    {
        var productOption = await _dbSet.Include(po => po.ProductOptionVariation).FirstOrDefaultAsync(po => po.Name.ToUpper() == productOptionName.ToUpper());
        if (productOption is null)
        {
            return null;
        }
        return productOption;
    }
}