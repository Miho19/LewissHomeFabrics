using FluentResults;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.ProductStrategy;
using Microsoft.Extensions.Logging;


namespace Lewiss.Pricing.Shared.Services;

public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SharedUtilityService _sharedUtilityService;

    private readonly ProductStrategyResolver _productStrategyResolver;
    private readonly ILogger<ProductService> _logger;


    public ProductService(IUnitOfWork unitOfWork,
    SharedUtilityService sharedUtilityService,
    ProductStrategyResolver productStrategyResolver,
    ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _sharedUtilityService = sharedUtilityService;
        _productStrategyResolver = productStrategyResolver;
        _logger = logger;
    }

    public virtual async Task<Result<ProductEntryOutputDTO>> CreateProductAsync(Guid externalCustomerId, Guid externalWorksheetId, ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {

        var productStategyResolverResult = _productStrategyResolver.GetProductStrategyByProductTypeString(productCreateDTO.ProductType);
        if (productStategyResolverResult.IsFailed)
        {
            return Result.Fail(productStategyResolverResult.Errors);
        }

        var customerAndWorksheetResult = await _sharedUtilityService.GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId);
        if (customerAndWorksheetResult.IsFailed)
        {
            return Result.Fail(customerAndWorksheetResult.Errors);
        }

        var (_, worksheet) = customerAndWorksheetResult.Value;

        var productStrategy = productStategyResolverResult.Value;

        var createProductResult = await productStrategy.CreateProductAsync(externalCustomerId, productCreateDTO, worksheet, cancellationToken);
        if (createProductResult.IsFailed)
        {
            return Result.Fail(createProductResult.Errors);
        }

        var product = createProductResult.Value;

        await _unitOfWork.Product.AddAsync(product);
        await _unitOfWork.CommitAsync();

        var productEntryDTO = product.ToProductEntryDTO(productCreateDTO);

        return productEntryDTO;


    }

    // there might be a better way to do this
    public virtual async Task<Result<List<ProductOptionVariation>>> PopulateProductOptionVariationList(object? obj, Type type, CancellationToken cancellationToken = default)
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

        var productStrategyResult = _productStrategyResolver.GetProductStrategyByProduct(product);
        if (productStrategyResult.IsFailed)
        {
            return Result.Fail(productStrategyResult.Errors);
        }

        var productStrategy = productStrategyResult.Value;

        var productEntryDTOResult = productStrategy.ProductToEntryDTO(product, externalWorksheetId);
        if (productEntryDTOResult.IsFailed)
        {
            return Result.Fail(productEntryDTOResult.Errors);
        }

        return Result.Ok(productEntryDTOResult.Value);


    }

    public decimal GetProductOptionVariationListTotalPrice(List<ProductOptionVariation> productOptionVariationList)
    {
        decimal total = 0.00m;
        foreach (var productOptionVariation in productOptionVariationList)
        {
            decimal price = productOptionVariation.Price.GetValueOrDefault();
            total += price;
        }

        return total;
    }

    public Result<Dictionary<string, object>> ProductOptionsToDictionary(Product product)
    {
        if (product.OptionVariations is null || product.OptionVariations.Count == 0)
        {
            return Result.Fail(new Error("Product Option Variations list is empty"));
        }

        var optionsDictionary = new Dictionary<string, object>();

        foreach (var op in product.OptionVariations)
        {
            if (op.ProductOption is null)
                continue;

            optionsDictionary.Add(op.ProductOption.Name, op.Value);
        }

        if (optionsDictionary.Count == 0)
        {
            return Result.Fail(new Error("Product Option Variations dictionary is empty"));
        }

        return Result.Ok(optionsDictionary);
    }

}

