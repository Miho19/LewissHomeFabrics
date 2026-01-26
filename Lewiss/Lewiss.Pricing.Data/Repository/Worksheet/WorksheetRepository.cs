using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository;
using Lewiss.Pricing.Data.Repository.Generic;
using Microsoft.EntityFrameworkCore;

public class WorksheetRepository : Repository<Worksheet>, IWorksheetRepository
{
    public WorksheetRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Worksheet>> GetWorksheetsByCustomerId(Guid customerId)
    {
        return await _dbSet.Where(w => w.CustomerId == customerId).ToListAsync();
    }
}