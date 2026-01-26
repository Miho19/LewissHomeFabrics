using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;
using Microsoft.EntityFrameworkCore;


namespace Lewiss.Pricing.Data.Repository.CustomerRepository;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Customer?> GetCustomerByExternalIdAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.ExternalId == externalCustomerId);
    }
}