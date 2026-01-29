using System.ComponentModel;
using System.Dynamic;
using System.Text.Json;
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

    // public ProductService(IUnitOfWork unitOfWork)
    // {
    //     _unitOfWork = unitOfWork;
    // }

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

        _logger.LogCritical("Got customer and worksheet");

        var product = await _unitOfWork.Product.GetProductByExternalIdAsync(externalProductId, cancellationToken);
        if (product is null)
        {
            _logger.LogCritical("failed retrieve product");
            return null;
        }

        var productEntryDTO = ProductToEntryDTO(product, externalWorksheetId);
        if (productEntryDTO is null)
        {
            _logger.LogCritical("failed to make dto");
            return null;
        }
        return productEntryDTO;
    }

    private Dictionary<string, object> ProductOptionsToDictionary(Data.Model.Product product)
    {
        var optionsDictionary = new Dictionary<string, object>();

        foreach (var op in product.OptionVariations)
        {
            if (op.ProductOption is null)
                continue;

            optionsDictionary.Add(op.ProductOption.Name, op.Value);
        }

        return optionsDictionary;
    }

    private FixedConfiguration? ProductOptionsDictionaryToFixedConfiguration(Dictionary<string, object> optionsDictionary)
    {
        var optionsDictionaryJsonString = JsonSerializer.Serialize(optionsDictionary);
        var fixedConfiguration = JsonSerializer.Deserialize<FixedConfiguration>(optionsDictionaryJsonString);

        return fixedConfiguration;
    }

    private VariableConfiguration ProductToVariableConfiguration(Data.Model.Product product)
    {
        var variableConfiguration = new VariableConfiguration()
        {
            Location = product.Location,
            Width = product.Width,
            Height = product.Height,
            Reveal = product.Reveal,
            RemoteNumber = product.RemoteNumber,
            RemoteChannel = product.RemoteChannel,
            InstallHeight = product.InstallHeight,
        };

        return variableConfiguration;
    }

    private ProductEntryDTO? ProductToEntryDTO(Data.Model.Product product, Guid externalWorksheetId)
    {

        var optionsDictionary = ProductOptionsToDictionary(product);

        var fixedConfiguration = ProductOptionsDictionaryToFixedConfiguration(optionsDictionary);
        if (fixedConfiguration is null)
        {
            return null;
        }


        var variableConfiguration = ProductToVariableConfiguration(product);
        if (variableConfiguration is null)
        {
            return null;
        }

        var productEntryDTO = new ProductEntryDTO
        {
            Id = product.ExternalMapping,
            WorksheetId = externalWorksheetId,
            Price = product.Price,
            VariableConfiguration = variableConfiguration,
            FixedConfiguration = fixedConfiguration,
        };

        productEntryDTO = ProductEntryDTOPopulateSpecificConfiguration(productEntryDTO, optionsDictionary);
        if (productEntryDTO is null)
        {
            return null;
        }

        return productEntryDTO;

    }

    private ProductEntryDTO? ProductEntryDTOPopulateSpecificConfiguration(ProductEntryDTO productEntryDTO, Dictionary<string, object> optionsDictionary)
    {

        var productType = productEntryDTO.FixedConfiguration.ProductType.ToUpper();
        productType = String.Concat(productType.Where(c => !Char.IsWhiteSpace(c)));


        var result = productType switch
        {
            "KINETICSCELLULAR" => ProductEntryDTOPopulateKineticsCellular(productEntryDTO, optionsDictionary),
            "KINETICSROLLER" => ProductEntryDTOPopulateKineticsRoller(productEntryDTO, optionsDictionary),
            _ => null,
        };

        if (result is null)
        {
            return null;
        }

        return result;
    }

    private ProductEntryDTO? ProductEntryDTOPopulateKineticsCellular(ProductEntryDTO productEntryDTO, Dictionary<string, object> optionsDictionary)
    {
        var optionsDictionaryJsonString = JsonSerializer.Serialize(optionsDictionary);
        var kineticsCellular = JsonSerializer.Deserialize<KineticsCellular>(optionsDictionaryJsonString);
        if (kineticsCellular is null)
        {
            return null;
        }

        productEntryDTO.KineticsCellular = kineticsCellular;
        return productEntryDTO;
    }
    private ProductEntryDTO? ProductEntryDTOPopulateKineticsRoller(ProductEntryDTO productEntryDTO, Dictionary<string, object> optionsDictionary)
    {
        var optionsDictionaryJsonString = JsonSerializer.Serialize(optionsDictionary);
        var kineticsRoller = JsonSerializer.Deserialize<KineticsRoller>(optionsDictionaryJsonString);
        if (kineticsRoller is null)
        {
            return null;
        }
        productEntryDTO.KineticsRoller = kineticsRoller;
        return productEntryDTO;
    }

}

