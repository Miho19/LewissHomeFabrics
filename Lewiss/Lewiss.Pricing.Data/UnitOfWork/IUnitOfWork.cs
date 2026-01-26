using Lewiss.Pricing.Data.Repository;
using Lewiss.Pricing.Data.Repository.CustomerRepository;

public interface IUnitOfWork : IDisposable
{
    IWorksheetRepository Worksheet {get;}
    ICustomerRepository Customer {get;}
    
    Task<int> CommitAsync();
}