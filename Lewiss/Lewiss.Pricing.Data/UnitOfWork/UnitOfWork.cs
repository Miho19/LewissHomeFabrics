using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Repository.CustomerRepository;
using Lewiss.Pricing.Data.Repository.WorksheetRepository;

public class UnitOfWork : IUnitOfWork
{

    private readonly PricingDbContext _pricingDbContext;
    public IWorksheetRepository Worksheet { get; private set; }

    public ICustomerRepository Customer { get; private set; }

    private bool _disposed = false;

    public UnitOfWork(PricingDbContext pricingDbContext, IWorksheetRepository worksheetRepository, ICustomerRepository customerRepository)
    {
        _pricingDbContext = pricingDbContext;
        Worksheet = worksheetRepository;
        Customer = customerRepository;
    }

    public async Task<int> CommitAsync()
    {
        return await _pricingDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _pricingDbContext.Dispose();
            }
        }
        _disposed = true;
    }

}