using Lewiss.Pricing.Data.Repository.CustomerRepository;
using Lewiss.Pricing.Data.Repository.ProductOptionRepository;
using Lewiss.Pricing.Data.Repository.ProductRepository;
using Lewiss.Pricing.Data.Repository.WorksheetRepository;

public interface IUnitOfWork : IDisposable
{
    IWorksheetRepository Worksheet { get; }
    ICustomerRepository Customer { get; }
    IProductRepository Product { get; }

    IKineticsCellularRepository KineticsCellularFabric { get; }

    IKineticsRollerRepository KineticsRollerFabric { get; }

    IProductOptionRepository ProductOption { get; set; }

    Task<int> CommitAsync();
}