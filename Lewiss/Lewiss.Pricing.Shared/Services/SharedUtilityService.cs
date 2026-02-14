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


    public string GetProductTypeQueryString(string productType)
    {
        return Regex.Replace(productType, @"\s+", String.Empty).ToLower();
    }

    // Will be replaced by interface / strategy pattern

    public Result<string> GetValidProductOptionTypeString(string productType)
    {
        var query = GetProductTypeQueryString(productType);

        var productTypeAdjusted = query switch
        {
            "kineticscellular" => ProductTypeOption.KineticsCellular.Value,
            "kineticsroller" => ProductTypeOption.KineticsRoller.Value,
            _ => "unknown"
        };

        if (productTypeAdjusted.Equals("unknown", StringComparison.CurrentCultureIgnoreCase))
        {
            return Result.Fail(new ValidationError("Product Type", productType));
        }

        return Result.Ok(productTypeAdjusted);

    }

    public Result<string> GetValidFabricOpacityStringForFabricPricing(string productType, string opacity)
    {
        var query = GetProductTypeQueryString(productType);

        var opacityAdjusted = query switch
        {
            "kineticscellular" => opacity,
            "kineticsroller" => GetValidKineticsRollerOpacityString(opacity),
            _ => "unknown"
        };

        if (opacityAdjusted.Equals("unknown", StringComparison.CurrentCultureIgnoreCase))
        {
            return Result.Fail(new ValidationError("Product Type", productType));
        }

        return Result.Ok(opacityAdjusted);
    }

    private string GetValidKineticsRollerOpacityString(string opacity)
    {
        if (opacity.Equals("ss", StringComparison.CurrentCultureIgnoreCase))
            return "SS";

        return "LF";

    }



}