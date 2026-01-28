using System.Reflection;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.Product;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;

namespace Lewiss.Pricing.Shared.Services;

public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // This will eventually be replaced by function in PricingService and Result pattern; currently this check is duplicated
    private async Task<(Data.Model.Customer?, Data.Model.Worksheet?)> GetCustomerAndWorksheetAsync(Guid externalCustomerId, Guid externalWorksheetId, CancellationToken cancellationToken = default)
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

    public virtual async Task<ProductEntryDTO?> CreateProductAsync(Guid externalCustomerId, Guid externalWorksheetId, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        var (customer, worksheet) = await GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId, cancellationToken);
        if (customer is null || worksheet is null)
        {
            return null;
        }

        var product = productCreateDTO.ToProductEntity(worksheet);

        product = await PopulateProductOptionVariationList_FixedConfigurationAsync(product, productCreateDTO, cancellationToken);
        if (product is null)
        {
            return null;
        }

        product = await PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(product, productCreateDTO, cancellationToken);
        if (product is null)
        {
            return null;
        }

        await _unitOfWork.Product.AddAsync(product);
        await _unitOfWork.CommitAsync();

        var productEntryDTO = product.ToProductEntryDTO(productCreateDTO);

        return productEntryDTO;
    }

    // Seems like a bad idea but taking in object and returning that object to signal wheather there was an error or not
    private async Task<Data.Model.Product?> PopulateProductOptionVariationList_FixedConfigurationAsync(Data.Model.Product product, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {

        Type fixedConfigurationType = typeof(FixedConfiguration);
        var fixedConfigurationTypeProperties = fixedConfigurationType.GetProperties();

        foreach (var property in fixedConfigurationTypeProperties)
        {
            var productOption = await _unitOfWork.ProductOption.GetProductOptionByNameAsync(property.Name, cancellationToken);
            if (productOption is null)
            {
                continue;
            }

            var propertyType = property.PropertyType;
            var propertyValue = property.GetValue(productCreateDTO.VariableConfiguration) as string;

            if (string.IsNullOrEmpty(propertyValue))
            {
                continue;
            }

            var productVariation = productOption.ProductOptionVariation.FirstOrDefault(pv => pv.Value.ToUpper() == propertyValue.ToUpper());
            if (productVariation is null)
            {
                return null;
            }

            product.OptionVariations.Add(productVariation);

        }

        return product;
    }


    private async Task<Data.Model.Product?> PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(Data.Model.Product product, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        var productType = productCreateDTO.FixedConfiguration.ProductType.ToUpper();
        productType = String.Concat(productType.Where(c => !Char.IsWhiteSpace(c)));

        var result = productType switch
        {
            "KINETICSCELLULAR" => await PopulateProductOptionVariationList_KineticsCellularAsync(product, productCreateDTO, cancellationToken),
            "KINETICSROLLER" => await PopulateProductOptionVariationList_KineticsRollerAsync(product, productCreateDTO, cancellationToken),
            _ => null,
        };

        if (result is null)
        {
            return null;
        }

        return product;
    }


    private async Task<Data.Model.Product?> PopulateProductOptionVariationList_KineticsCellularAsync(Data.Model.Product product, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {

        return null;
    }

    private async Task<Data.Model.Product?> PopulateProductOptionVariationList_KineticsRollerAsync(Data.Model.Product product, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        return null;
    }

}

