using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Shared.Services;

public class SharedUtilityService
{
    private readonly IUnitOfWork _unitOfWork;
    public SharedUtilityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(Customer?, Worksheet?)> GetCustomerAndWorksheetAsync(Guid externalCustomerId, Guid externalWorksheetId, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(externalCustomerId, cancellationToken);
        if (customer is null)
        {
            return (null, null);
        }

        var worksheet = await _unitOfWork.Worksheet.GetWorksheetByExternalIdAsync(externalWorksheetId, cancellationToken);
        if (worksheet is null)
        {
            return (null, null);
        }

        if (worksheet.CustomerId != customer.CustomerId)
        {
            return (null, null);
        }

        return (customer, worksheet);
    }

}