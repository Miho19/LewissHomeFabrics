using System.Text.RegularExpressions;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.OptionData;

namespace Lewiss.Pricing.Shared.Services;

public class SharedUtilityService
{
    private readonly IUnitOfWork _unitOfWork;
    public SharedUtilityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<(Customer, Worksheet)> GetCustomerAndWorksheetAsync(Guid externalCustomerId, Guid externalWorksheetId, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(externalCustomerId, cancellationToken);
        if (customer is null)
        {
            throw new Exception("Customer not found");
        }

        var worksheet = await _unitOfWork.Worksheet.GetWorksheetByExternalIdAsync(externalWorksheetId, cancellationToken);
        if (worksheet is null)
        {
            throw new Exception("Worksheet not found");
        }

        if (worksheet.CustomerId != customer.CustomerId)
        {
            throw new Exception("Customer Id does not match Worksheet Customer Id");
        }

        return (customer, worksheet);
    }


    public string GetProductTypeQueryString(string productType)
    {
        return Regex.Replace(productType, @"\s+", String.Empty).ToLower();
    }

    public string GetValidProductOptionTypeString(string productType)
    {
        var query = GetProductTypeQueryString(productType);

        var productTypeAdjusted = query switch
        {
            "kineticscellular" => ProductTypeOption.KineticsCellular.Value,
            "kineticsroller" => ProductTypeOption.KineticsRoller.Value,
            _ => throw new Exception("Product Type supplied does not match any from the database"),
        };

        return productTypeAdjusted;

    }



}