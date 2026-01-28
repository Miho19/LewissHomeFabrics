using Lewiss.Pricing.Shared.Product;
using Microsoft.Extensions.Logging;

namespace Lewiss.Pricing.Shared.Services;

public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
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
        // product = await PopulateProductOptionVariationList_FixedConfigurationAsync(product, productCreateDTO, cancellationToken);
        if (product is null)
        {
            return null;
        }

        // product = await PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(product, productCreateDTO, cancellationToken);
        // if (product is null)
        // {
        //     return null;
        // }

        await _unitOfWork.Product.AddAsync(product);
        await _unitOfWork.CommitAsync();

        var productEntryDTO = product.ToProductEntryDTO(productCreateDTO);

        return productEntryDTO;
    }

    private async Task<Data.Model.Product?> PopulateProductOptionVariationListByType(Data.Model.Product product, object obj, Type type, CancellationToken cancellationToken = default)
    {
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

                Type propertyType = property.PropertyType;
                object value = Convert.ChangeType(propertyValue, propertyType);

#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
                var productVariation = productOption.ProductOptionVariation.FirstOrDefault(pv => pv.Value.ToString().ToUpper() == propertyValue?.ToString()?.ToUpper());
                _logger.LogCritical($"product variation: {productVariation is null} value: {value}");
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast
                if (productVariation is null)
                {
                    return null;
                }

                product.OptionVariations.Add(productVariation);


            }
            catch (Exception)
            {
                return null;
            }

        }

        return product;
    }

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

