using Lewiss.Pricing.Data.Repository.CustomerRepository;
using Lewiss.Pricing.Data.Repository.Fabric;
using Lewiss.Pricing.Data.Repository.ProductOptionRepository;
using Lewiss.Pricing.Data.Repository.ProductRepository;
using Lewiss.Pricing.Data.Repository.WorksheetRepository;

public interface IUnitOfWork : IDisposable
{
    IWorksheetRepository Worksheet { get; }
    ICustomerRepository Customer { get; }
    IProductRepository Product { get; }

    IFabricPriceRepository FabricPrice { get; }

    IKineticsCellularFabricRepository KineticsCellularFabric { get; }

    IKineticsRollerFabricRepository KineticsRollerFabric { get; }

    IProductOptionRepository ProductOption { get; }

    Task<int> CommitAsync();
}