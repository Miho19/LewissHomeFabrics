using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.ProductDTO;
using Microsoft.Extensions.Logging;


namespace Lewiss.Pricing.Shared.Services;

public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SharedUtilityService _sharedUtilityService;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IUnitOfWork unitOfWork, SharedUtilityService sharedUtilityService, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _sharedUtilityService = sharedUtilityService;
        _logger = logger;
    }



    // Need to adjust this to follow a more functional programming approach
    public virtual async Task<ProductEntryOutputDTO?> CreateProductAsync(Guid externalCustomerId, Guid externalWorksheetId, ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        try
        {
            var (customer, worksheet) = await _sharedUtilityService.GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId, cancellationToken);


            var product = productCreateDTO.ToProductEntity(worksheet);

            var generalProductOptionVariationList = await PopulateProductOptionVariationList(productCreateDTO.FixedConfiguration, typeof(FixedConfiguration), cancellationToken);


            var productTypeSpecificProductOptionVariationList = await PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(productCreateDTO, cancellationToken);


            //  Go through option variations --> add their price to total
            // Get fabric price info --> add to price totalv 

            await _unitOfWork.Product.AddAsync(product);
            await _unitOfWork.CommitAsync();

            var productEntryDTO = product.ToProductEntryDTO(productCreateDTO);

            return productEntryDTO;

        }
        catch (Exception ex)
        {
            _logger.LogError($"CreateProductAsync Exception: {ex.Message}");
            return null;
        }





    }

    private async Task<List<ProductOptionVariation>> PopulateProductOptionVariationList(object? obj, Type type, CancellationToken cancellationToken = default)
    {
        if (obj is null)
        {
            throw new Exception("Input object is null");
        }

        List<ProductOptionVariation> productOptionVariations = [];

        var typeProperties = type.GetProperties();
        foreach (var property in typeProperties)
        {
            var productOption = await _unitOfWork.ProductOption.GetProductOptionByNameAsync(property.Name, cancellationToken);
            if (productOption is null)
            {
                continue;
            }

            var propertyValue = property.GetValue(obj);
            if (propertyValue is null)
            {
                continue;
            }

            var productVariation = productOption.ProductOptionVariation.FirstOrDefault(pv => pv.Value.ToString().ToUpper() == propertyValue?.ToString()?.ToUpper());
            if (productVariation is null)
            {
                throw new Exception($"For Option {productOption}, {propertyValue} is not a valid value");
            }

            productOptionVariations.Add(productVariation);

        }

        return productOptionVariations;

    }



    private async Task<List<ProductOptionVariation>> PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {

        var queryProductType = _sharedUtilityService.GetProductTypeQueryString(productCreateDTO.FixedConfiguration.ProductType);

        var result = queryProductType switch
        {
            "kineticscellular" => await PopulateProductOptionVariationList(productCreateDTO.KineticsCellular, typeof(KineticsCellular), cancellationToken),
            "kineticsroller" => await PopulateProductOptionVariationList(productCreateDTO.KineticsRoller, typeof(KineticsRoller), cancellationToken),
            _ => throw new Exception("Invalid Product Type"),
        };

        return result;
    }


    public virtual async Task<ProductEntryOutputDTO?> GetProductAsync(Guid externalCustomerId, Guid externalWorksheetId, Guid externalProductId, CancellationToken cancellationToken = default)
    {
        try
        {
            var (customer, worksheet) = await _sharedUtilityService.GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId, cancellationToken);

            var product = await _unitOfWork.Product.GetProductByExternalIdAsync(externalProductId, cancellationToken);
            if (product is null)
            {
                throw new Exception($"Failed to retrieve product external Id: {externalProductId}");
            }

            var productEntryDTO = product.ToProductEntryDTO(externalWorksheetId);

            return productEntryDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ProductService.GetProductAsync exception: {ex.Message}");
            return null;
        }

    }



}

