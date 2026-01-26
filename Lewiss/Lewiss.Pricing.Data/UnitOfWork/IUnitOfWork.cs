using Lewiss.Pricing.Data.Repository.CustomerRepository;
using Lewiss.Pricing.Data.Repository.ProductRepository;
using Lewiss.Pricing.Data.Repository.WorksheetRepository;

public interface IUnitOfWork : IDisposable
{
    IWorksheetRepository Worksheet { get; }
    ICustomerRepository Customer { get; }

    IProductRepository Product { get; }

    Task<int> CommitAsync();
}