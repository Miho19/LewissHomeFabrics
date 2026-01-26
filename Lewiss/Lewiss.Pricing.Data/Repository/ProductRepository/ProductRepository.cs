using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;

namespace Lewiss.Pricing.Data.Repository.ProductRepository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }


}