using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Repository.Generic;

namespace Lewiss.Pricing.Data.Repository.CustomerRepository;
public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetCustomerByExternalIdAsync(Guid externalCustomerId, CancellationToken cancellationToken);
    Task<List<Customer>> GetCustomerByQueryableParameters(string? familyName, string? mobile, string? email, CancellationToken cancellationToken);
}