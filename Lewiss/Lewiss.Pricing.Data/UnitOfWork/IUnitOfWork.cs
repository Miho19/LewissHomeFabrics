using Lewiss.Pricing.Data.Repository.CustomerRepository;
using Lewiss.Pricing.Data.Repository.WorksheetRepository;

public interface IUnitOfWork : IDisposable
{
    IWorksheetRepository Worksheet { get; }
    ICustomerRepository Customer { get; }

    Task<int> CommitAsync();
}