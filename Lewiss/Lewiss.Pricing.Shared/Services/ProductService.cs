using Lewiss.Pricing.Data.Model;
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

            var generalProductOptionVariationList = await PopulateGeneralProductOptionVariationList(productCreateDTO.FixedConfiguration, typeof(FixedConfiguration), cancellationToken);


            product = await PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(product, productCreateDTO, cancellationToken);
            if (product is null)
            {
                return null;
            }

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

    private async Task<List<ProductOptionVariation>> PopulateGeneralProductOptionVariationList(object? obj, Type type, CancellationToken cancellationToken = default)
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



    private async Task<List<ProductOptionVariation>> PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(Data.Model.Product product, ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        try
        {
            var productType = productCreateDTO.FixedConfiguration.ProductType.ToLower();
            productType = String.Concat(productType.Where(c => !Char.IsWhiteSpace(c)));

            var result = productType switch
            {
                "kineticscellular" => await PopulateProductOptionVariationListByType(product, productCreateDTO.KineticsCellular, typeof(KineticsCellular), cancellationToken),
                "kineticsroller" => await PopulateProductOptionVariationListByType(product, productCreateDTO.KineticsRoller, typeof(KineticsRoller), cancellationToken),
                _ => null,
            };
        }
        catch (Exception ex)
        {

        }




        return product;
    }


    public virtual async Task<ProductEntryOutputDTO?> GetProductAsync(Guid externalCustomerId, Guid externalWorksheetId, Guid externalProductId, CancellationToken cancellationToken = default)
    {
        var (customer, worksheet) = await _sharedUtilityService.GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId, cancellationToken);
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

