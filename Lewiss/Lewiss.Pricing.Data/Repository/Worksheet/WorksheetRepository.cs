using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository;
using Lewiss.Pricing.Data.Repository.Generic;
using Microsoft.EntityFrameworkCore;

public class WorksheetRepository : Repository<Worksheet>, IWorksheetRepository
{
    public WorksheetRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Worksheet>> GetWorksheetsByExternalCustomerId(Guid externalCustomerId)
    {
        var customer = _dbContext.Set<Customer>().FirstOrDefaultAsync(c => c.ExternalId == externalCustomerId);

        return await _dbSet.Where(w => w.CustomerId == customer.Id).ToListAsync();
    }
}