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

    public async Task<Worksheet?> GetWorksheetByExternalIdAsync(Guid externalWorksheetId, CancellationToken cancellationToken)
    {
        var worksheet = await _dbContext.Set<Worksheet>().FirstOrDefaultAsync(w => w.ExternalMapping == externalWorksheetId);
        return worksheet;
    }

    public async Task<List<Worksheet>?> GetWorksheetsByExternalCustomerIdAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        var customer = _dbContext.Set<Customer>().FirstOrDefaultAsync(c => c.ExternalMapping == externalCustomerId);
        if (customer is null)
        {
            return null;
        }

        var worksheets = await _dbContext.Set<Worksheet>().Where(w => w.CustomerId == customer.Id).ToListAsync();

        if (worksheets is null || worksheets.Count == 0)
        {
            return [];
        }

        return worksheets;
    }
}