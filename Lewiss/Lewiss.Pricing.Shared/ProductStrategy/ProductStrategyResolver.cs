using System.Text.RegularExpressions;
using FluentResults;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.CustomError;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;

namespace Lewiss.Pricing.Shared.ProductStrategy;

public class ProductStrategyResolver
{
    private readonly IServiceProvider _serviceProvider;
    public ProductStrategyResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Result<IProductStrategy> GetProductStrategyByProductTypeString(string productType)
    {

        var productTypeAdjusted = Regex.Replace(productType, @"\s+", String.Empty).ToLower();
        if (productTypeAdjusted.Equals(ProductTypeOption.KineticsRoller.Value, StringComparison.CurrentCultureIgnoreCase))
        {
            return Result.Ok((IProductStrategy)_serviceProvider.GetRequiredService<KineticsRollerProductStrategy>());
        }

        if (productTypeAdjusted.Equals(ProductTypeOption.KineticsCellular.Value, StringComparison.CurrentCultureIgnoreCase))
        {
            return Result.Ok((IProductStrategy)_serviceProvider.GetRequiredService<KineticsCellularProductStrategy>());
        }

        return Result.Fail(new ValidationError("Product Type", productType));
    }

    // These are internal errors because product should come from the database...
    public Result<IProductStrategy> GetProductStrategyByProduct(Product product)
    {
        if (product is null)
        {
            return Result.Fail(new Error("Product is null"));
        }

        if (product.OptionVariations is null || product.OptionVariations.Count == 0)
        {
            return Result.Fail(new Error("Product Option Variations is null or empty"));
        }

        var productTypeOptionVariation = product.OptionVariations.FirstOrDefault(ov => ov.ProductOptionId == ProductTypeOption.ProductOption.ProductOptionId);
        if (productTypeOptionVariation is null)
        {
            return Result.Fail(new Error("Product Type Option Variation is null"));
        }

        if (productTypeOptionVariation.Value.Equals(ProductTypeOption.KineticsRoller.Value, StringComparison.CurrentCultureIgnoreCase))
        {
            return Result.Ok((IProductStrategy)_serviceProvider.GetRequiredService<KineticsRollerProductStrategy>());

        }

        if (productTypeOptionVariation.Value.Equals(ProductTypeOption.KineticsCellular.Value, StringComparison.CurrentCultureIgnoreCase))
        {
            return Result.Ok((IProductStrategy)_serviceProvider.GetRequiredService<KineticsCellularProductStrategy>());

        }

        return Result.Fail(new Error("Unknown Product Type"));

    }

}