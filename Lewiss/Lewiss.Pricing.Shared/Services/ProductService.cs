using FluentResults;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.CustomError;
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

    /** 
        When creating product
        0. Validate the product type and use strategy interface
        1. Check to make sure customer owns worksheet
        2. convert create dto to product model
        3. 
    */

    public virtual async Task<Result<ProductEntryOutputDTO>> CreateProductAsync(Guid externalCustomerId, Guid externalWorksheetId, ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {


        var result = await _sharedUtilityService.GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId);
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        var (_, worksheet) = result.Value;

        var productModel = productCreateDTO.ToProductEntity(worksheet);

        var populateGeneralConfigurationResult = await PopulateProductOptionVariationList(productCreateDTO, typeof(ProductCreateInputDTO), cancellationToken);
        if (populateGeneralConfigurationResult.IsFailed)
        {
            return Result.Fail(populateGeneralConfigurationResult.Errors);
        }

        var GeneralConfigurationList = populateGeneralConfigurationResult.Value;


        var productTypeSpecificProductOptionVariationListResult = await PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(productCreateDTO, cancellationToken);
        if (productTypeSpecificProductOptionVariationListResult.IsFailed)
        {
            return Result.Fail(productTypeSpecificProductOptionVariationListResult.Errors);
        }

        var productTypeSpecificProductOptionVariationList = productTypeSpecificProductOptionVariationListResult.Value;

        product.OptionVariations = [.. GeneralConfigurationList, .. productTypeSpecificProductOptionVariationList];

        // Get fabric price info --> add to price total

        var totalPriceProductOptionVariationList = GetProductOptionVariationListTotalPrice(product.OptionVariations.ToList());
        var fabricPriceResult = await GetProductFabricPriceOutputDTO(product.OptionVariations.ToList(), product.Width, product.Height, cancellationToken);
        if (fabricPriceResult.IsFailed)
        {
            return Result.Fail(fabricPriceResult.Errors);
        }

        var fabricPrice = fabricPriceResult.Value;


        product.Price = totalPriceProductOptionVariationList + fabricPrice.Price;


        await _unitOfWork.Product.AddAsync(product);
        await _unitOfWork.CommitAsync();

        var productEntryDTO = product.ToProductEntryDTO(productCreateDTO);

        return productEntryDTO;


    }

    private async Task<Result<FabricPriceOutputDTO>> GetProductFabricPriceOutputDTO(List<ProductOptionVariation> productOptionVariations, int width, int height, CancellationToken cancellationToken = default)
    {
        var fabricProductOptionVariation = productOptionVariations.FirstOrDefault(ov => ov.ProductOptionId == FabricOption.ProductOption.ProductOptionId);
        if (fabricProductOptionVariation is null)
        {
            // this should never be null but i have to figure out why i put this test here
            return Result.Fail(new ValidationError("Fabric", "null"));
        }

        var productTypeProductOptionVariation = productOptionVariations.FirstOrDefault(ov => ov.ProductOptionId == ProductTypeOption.ProductOption.ProductOptionId);
        if (productTypeProductOptionVariation is null)
        {
            return Result.Fail(new ValidationError("Product Type", "null"));
        }

        var fabricPriceOutputDTOResult = await _fabricService.GetFabricPriceOutputDTOByProductOptionVariationIdAsync(productTypeProductOptionVariation.Value, fabricProductOptionVariation.ProductOptionVariationId, width, height, cancellationToken);
        if (fabricPriceOutputDTOResult.IsFailed)
        {
            return Result.Fail(fabricPriceOutputDTOResult.Errors);
        }
        return Result.Ok(fabricPriceOutputDTOResult.Value);
    }

    private async Task<Result<List<ProductOptionVariation>>> PopulateProductOptionVariationList(object? obj, Type type, CancellationToken cancellationToken = default)
    {
        if (obj is null)
        {
            return Result.Fail(new ValidationError(type.Name, "null"));

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
                return Result.Fail(new ValidationError(productOption.Name, propertyValue));
            }

            productOptionVariations.Add(productVariation);

        }

        return Result.Ok(productOptionVariations);

    }



    private async Task<Result<List<ProductOptionVariation>>> PopulateProductOptionVariationList_ProductTypeSpecificConfigurationAsync(ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {

        var queryProductType = _sharedUtilityService.GetProductTypeQueryString(productCreateDTO.FixedConfiguration.ProductType);

        var result = queryProductType switch
        {
            "kineticscellular" => await PopulateProductOptionVariationList(productCreateDTO.KineticsCellular, typeof(KineticsCellular), cancellationToken),
            "kineticsroller" => await PopulateProductOptionVariationList(productCreateDTO.KineticsRoller, typeof(KineticsRoller), cancellationToken),
            _ => Result.Fail(new ValidationError("Product Type", productCreateDTO.FixedConfiguration.ProductType)),
        };

        return result;
    }


    public virtual async Task<Result<ProductEntryOutputDTO>> GetProductAsync(Guid externalCustomerId, Guid externalWorksheetId, Guid externalProductId, CancellationToken cancellationToken = default)
    {

        var result = await _sharedUtilityService.GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId);
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        var product = await _unitOfWork.Product.GetProductByExternalIdAsync(externalProductId, cancellationToken);
        if (product is null)
        {
            return Result.Fail(new NotFoundResource("Product", externalProductId));
        }

        var productEntryDTO = product.ToProductEntryDTO(externalWorksheetId);

        return Result.Ok(productEntryDTO);


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

