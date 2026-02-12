using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.Error;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.ProductDTO;
using Microsoft.Extensions.Logging;


namespace Lewiss.Pricing.Shared.Services;

public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SharedUtilityService _sharedUtilityService;
    private readonly ILogger<ProductService> _logger;
    private readonly FabricService _fabricService;

    public ProductService(IUnitOfWork unitOfWork, SharedUtilityService sharedUtilityService, FabricService fabricService, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _sharedUtilityService = sharedUtilityService;
        _fabricService = fabricService;
        _logger = logger;
    }

    public virtual async Task<ProductEntryOutputDTO?> CreateProductAsync(Guid externalCustomerId, Guid externalWorksheetId, ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {

        var (_, worksheet) = await _sharedUtilityService.GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId, cancellationToken);

        var product = productCreateDTO.ToProductEntity(worksheet);

        var generalProductOptionVariationList = await PopulateProductOptionVariationList(productCreateDTO.FixedConfiguration, typeof(FixedConfiguration), cancellationToken);


        var productTypeSpecificProductOptionVariationList = await PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(productCreateDTO, cancellationToken);

        product.OptionVariations = [.. generalProductOptionVariationList, .. productTypeSpecificProductOptionVariationList];

        // Get fabric price info --> add to price total

        var totalPriceProductOptionVariationList = GetProductOptionVariationListTotalPrice(product.OptionVariations.ToList());
        var fabricPrice = await GetProductFabricPriceOutputDTO(product.OptionVariations.ToList(), product.Width, product.Height, cancellationToken);

        product.Price = totalPriceProductOptionVariationList + fabricPrice.Price;


        await _unitOfWork.Product.AddAsync(product);
        await _unitOfWork.CommitAsync();

        var productEntryDTO = product.ToProductEntryDTO(productCreateDTO);

        return productEntryDTO;


    }

    private async Task<FabricPriceOutputDTO> GetProductFabricPriceOutputDTO(List<ProductOptionVariation> productOptionVariations, int width, int height, CancellationToken cancellationToken = default)
    {
        var fabricProductOptionVariation = productOptionVariations.FirstOrDefault(ov => ov.ProductOptionId == FabricOption.ProductOption.ProductOptionId);
        if (fabricProductOptionVariation is null)
        {
            throw new InvalidQueryParameterException("Fabric must be supplied");
        }

        var productTypeProductOptionVariation = productOptionVariations.FirstOrDefault(ov => ov.ProductOptionId == ProductTypeOption.ProductOption.ProductOptionId);
        if (productTypeProductOptionVariation is null)
        {
            throw new InvalidQueryParameterException("Product type is invalid");
        }

        var fabricPriceOutputDTO = await _fabricService.GetFabricPriceOutputDTOByProductOptionVariationIdAsync(productTypeProductOptionVariation.Value, fabricProductOptionVariation.ProductOptionVariationId, width, height, cancellationToken);

        return fabricPriceOutputDTO;
    }

    private async Task<List<ProductOptionVariation>> PopulateProductOptionVariationList(object? obj, Type type, CancellationToken cancellationToken = default)
    {
        if (obj is null)
        {
            throw new InternalSystemException("Something went wrong");

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

            var valueAsString = (string)propertyValue;

            var productVariation = productOption.ProductOptionVariation.FirstOrDefault(pv => pv.Value == valueAsString);
            if (productVariation is null)
            {
                throw new InvalidQueryParameterException($"{propertyValue} is not valid for {productOption.Name}");

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
            _ => throw new InvalidQueryParameterException("Product type is invalid")
        };

        return result;
    }


    public virtual async Task<ProductEntryOutputDTO?> GetProductAsync(Guid externalCustomerId, Guid externalWorksheetId, Guid externalProductId, CancellationToken cancellationToken = default)
    {

        var (customer, worksheet) = await _sharedUtilityService.GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId, cancellationToken);

        var product = await _unitOfWork.Product.GetProductByExternalIdAsync(externalProductId, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException($"Product not found\nid: {externalProductId}");
        }

        var productEntryDTO = product.ToProductEntryDTO(externalWorksheetId);

        return productEntryDTO;


    }

    private decimal GetProductOptionVariationListTotalPrice(List<ProductOptionVariation> productOptionVariationList)
    {
        decimal total = 0.00m;
        foreach (var productOptionVariation in productOptionVariationList)
        {
            decimal price = productOptionVariation.Price.GetValueOrDefault();
            total += price;
        }

        return total;
    }



}

