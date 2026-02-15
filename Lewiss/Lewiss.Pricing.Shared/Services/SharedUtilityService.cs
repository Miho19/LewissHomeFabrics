using System.Text.RegularExpressions;
using FluentResults;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.CustomError;


namespace Lewiss.Pricing.Shared.Services;

public class SharedUtilityService
{
    private readonly IUnitOfWork _unitOfWork;
    public SharedUtilityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<(Customer, Worksheet)>> GetCustomerAndWorksheetAsync(Guid externalCustomerId, Guid externalWorksheetId, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(externalCustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Fail(new NotFoundResource("Customer", externalCustomerId));
        }

        var worksheet = await _unitOfWork.Worksheet.GetWorksheetByExternalIdAsync(externalWorksheetId, cancellationToken);
        if (worksheet is null)
        {
            return Result.Fail(new NotFoundResource("Worksheet", externalWorksheetId));

        }

        if (worksheet.CustomerId != customer.CustomerId)
        {
            return Result.Fail(new ResourceNotOwned("Customer", externalCustomerId, "Worksheet", externalWorksheetId));
        }

        return Result.Ok((customer, worksheet));
    }


}