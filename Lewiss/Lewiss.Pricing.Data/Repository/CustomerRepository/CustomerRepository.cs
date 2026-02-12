using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;
using Microsoft.EntityFrameworkCore;


namespace Lewiss.Pricing.Data.Repository.CustomerRepository;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Customer?> GetCustomerByExternalIdAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.ExternalMapping == externalCustomerId, cancellationToken);
    }

    public async Task<List<Customer>> GetCustomerByQueryableParameters(string? familyName, string? mobile, string? email, CancellationToken cancellationToken = default)
    {
        IQueryable<Customer> query = _dbSet;

        if (!string.IsNullOrEmpty(familyName))
        {
            query = query.Where(c => c.FamilyName == familyName);
        }

        if (!string.IsNullOrEmpty(mobile))
        {
            query = query.Where(c => c.Mobile == mobile);
        }

        if (!string.IsNullOrEmpty(email))
        {
            query = query.Where(c => c.Email == email);
        }

        return await query.ToListAsync(cancellationToken);
    }
}