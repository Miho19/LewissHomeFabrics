using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lewiss.Pricing.Data.Repository.WorksheetRepository;

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
        var customer = await _dbContext.Set<Customer>().Include(c => c.CurrentWorksheets).FirstOrDefaultAsync(c => c.ExternalMapping == externalCustomerId, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var worksheets = customer.CurrentWorksheets.ToList();

        if (worksheets is null || worksheets.Count == 0)
        {
            return [];
        }

        return worksheets;
    }

    public async Task<List<Product>> GetWorksheetProductsAsync(Worksheet worksheet, CancellationToken cancellationToken)
    {
        var productList = await _dbContext.Product
        .Include(p => p.OptionVariations)
        .Where(p => p.WorksheetId == worksheet.WorksheetId)
        .ToListAsync();

        return productList;
    }


}