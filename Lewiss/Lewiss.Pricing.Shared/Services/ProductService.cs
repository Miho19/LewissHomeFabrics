using System.ComponentModel;
using System.Dynamic;
using System.Text.Json;
using Lewiss.Pricing.Shared.Product;
using Microsoft.Extensions.Logging;

namespace Lewiss.Pricing.Shared.Services;

public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    // private readonly ILogger<ProductService> _logger;

    // public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger)
    // {
    //     _unitOfWork = unitOfWork;
    //     _logger = logger;
    // }

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

        product = await PopulateProductOptionVariationListByType(product, productCreateDTO.FixedConfiguration, typeof(FixedConfiguration), cancellationToken);
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

    private async Task<Data.Model.Product?> PopulateProductOptionVariationListByType(Data.Model.Product product, object? obj, Type type, CancellationToken cancellationToken = default)
    {
        if (obj is null)
        {
            return null;
        }

        var typeProperties = type.GetProperties();
        foreach (var property in typeProperties)
        {
            var productOption = await _unitOfWork.ProductOption.GetProductOptionByNameAsync(property.Name, cancellationToken);
            if (productOption is null)
            {
                continue;
            }

            try
            {
                var propertyValue = property.GetValue(obj);
                if (propertyValue is null)
                {
                    continue;
                }

                var productVariation = productOption.ProductOptionVariation.FirstOrDefault(pv => pv.Value.ToString().ToUpper() == propertyValue?.ToString()?.ToUpper());
                if (productVariation is null)
                {
                    return null;
                }

                product.OptionVariations.Add(productVariation);

            }
            catch
            {

                return null;
            }

        }

        return product;
    }

    private async Task<Data.Model.Product?> PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(Data.Model.Product product, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        var productType = productCreateDTO.FixedConfiguration.ProductType.ToUpper();
        productType = String.Concat(productType.Where(c => !Char.IsWhiteSpace(c)));

        var result = productType switch
        {
            "KINETICSCELLULAR" => await PopulateProductOptionVariationListByType(product, productCreateDTO.KineticsCellular, typeof(KineticsCellular), cancellationToken),
            "KINETICSROLLER" => await PopulateProductOptionVariationListByType(product, productCreateDTO.KineticsRoller, typeof(KineticsRoller), cancellationToken),
            _ => null,
        };

        if (result is null)
        {
            return null;
        }

        return product;
    }


    public virtual async Task<ProductEntryDTO?> GetProductAsync(Guid externalCustomerId, Guid externalWorksheetId, Guid externalProductId, CancellationToken cancellationToken = default)
    {
        var (customer, worksheet) = await GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId, cancellationToken);
        if (customer is null || worksheet is null)
        {
            return null;
        }



        var product = await _unitOfWork.Product.GetProductByExternalIdAsync(externalProductId, cancellationToken);
        if (product is null)
        {
            return null;
        }

        var productEntryDTO = product.ToProductEntryDTO(externalWorksheetId);

        return productEntryDTO;
    }



}

