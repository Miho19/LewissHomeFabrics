using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;

namespace Lewiss.Pricing.Data.Repository.WorksheetRepository;

public interface IWorksheetRepository : IRepository<Worksheet>
{
    Task<List<Worksheet>?> GetWorksheetsByExternalCustomerIdAsync(Guid externalCustomerId, CancellationToken cancellationToken);

    Task<Worksheet?> GetWorksheetByExternalIdAsync(Guid externalWorksheetId, CancellationToken cancellationToken);

    Task<List<Product>> GetWorksheetProductsAsync(Worksheet worksheet, CancellationToken cancellationToken);

}