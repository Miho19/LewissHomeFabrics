using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;

namespace Lewiss.Pricing.Data.Repository.ProductOptionRepository;

public interface IProductOptionRepository : IRepository<ProductOption>
{
    Task<ProductOption?> GetProductOptionByNameAsync(string productOptionName, CancellationToken cancellationToken);
}