using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;
using Lewiss.Pricing.Data.Repository.WorksheetRepository;
using Microsoft.EntityFrameworkCore;

public class WorksheetRepository : Repository<Worksheet>, IWorksheetRepository
{
    public WorksheetRepository(PricingDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Worksheet?> GetWorksheetByExternalIdAsync(Guid externalWorksheetId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Worksheet>?> GetWorksheetsByExternalCustomerIdAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        var customer = _dbContext.Set<Customer>().FirstOrDefaultAsync(c => c.ExternalMapping == externalCustomerId);

        return await _dbSet.Where(w => w.CustomerId == customer.Id).ToListAsync();
    }
}