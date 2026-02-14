using System.Text.RegularExpressions;
using FluentResults;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.CustomError;
using Microsoft.Extensions.DependencyInjection;

namespace Lewiss.Pricing.Shared.ProductStrategy;

public class ProductStrategyResolver
{
    private readonly IServiceProvider _serviceProvider;
    public ProductStrategyResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Result<IProductStrategy<T>> GetProductStrategy<T>(string productType)
    {

        var productTypeAdjusted = Regex.Replace(productType, @"\s+", String.Empty).ToLower();
        if (productTypeAdjusted.Equals(ProductTypeOption.KineticsRoller.Value, StringComparison.CurrentCultureIgnoreCase))
        {
            return Result.Ok((IProductStrategy<T>)_serviceProvider.GetRequiredService<KineticsRollerProductStrategy>());
        }

        if (productTypeAdjusted.Equals(ProductTypeOption.KineticsCellular.Value, StringComparison.CurrentCultureIgnoreCase))
        {
            return Result.Ok((IProductStrategy<T>)_serviceProvider.GetRequiredService<KineticsCellularProductStrategy>());
        }

        return Result.Fail(new ValidationError("Product Type", productType));
    }

}