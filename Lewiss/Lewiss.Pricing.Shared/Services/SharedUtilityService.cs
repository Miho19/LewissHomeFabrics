using System.Text.RegularExpressions;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.Error;

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
            throw new NotFoundException($"Customer not found by id: {externalCustomerId}");
        }

        var worksheet = await _unitOfWork.Worksheet.GetWorksheetByExternalIdAsync(externalWorksheetId, cancellationToken);
        if (worksheet is null)
        {
            throw new NotFoundException($"Worksheet not found by id: {externalWorksheetId}");
        }

        if (worksheet.CustomerId != customer.CustomerId)
        {
            throw new InvalidQueryParameterException("Worksheet does not belong to Customer");
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
            _ => throw new InvalidQueryParameterException("Product type is invalid"),
        };

        return productTypeAdjusted;

    }

    public string GetValidFabricOpacityStringForFabricPricing(string productType, string opacity)
    {
        var query = GetProductTypeQueryString(productType);

        var opacityAdjusted = query switch
        {
            "kineticscellular" => opacity,
            "kineticsroller" => GetValidKineticsRollerOpacityString(opacity),
            _ => throw new InvalidQueryParameterException("Product type is invalid"),
        };

        return opacityAdjusted;
    }

    private string GetValidKineticsRollerOpacityString(string opacity)
    {
        if (opacity == "BO")
        {
            opacity = "LF";
        }

        return opacity;
    }



}