using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;

namespace Lewiss.Pricing.Data.Repository;

public interface IWorksheetRepository : IRepository<Worksheet>
{
    Task<List<Worksheet>> GetWorksheetsByExternalCustomerId(Guid externalCustomerId);
}