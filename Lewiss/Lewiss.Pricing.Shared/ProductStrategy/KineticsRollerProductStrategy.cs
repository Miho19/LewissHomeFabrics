using System.Text.Json;
using FluentResults;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.Services;

namespace Lewiss.Pricing.Shared.ProductStrategy;


public class KineticsRollerProductStrategy : IProductStrategy
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductService _productService;

    private readonly SharedUtilityService _sharedUtilityService;

    public string ProductType => ProductTypeOption.KineticsRoller.Value;

    public KineticsRollerProductStrategy(IUnitOfWork unitOfWork, ProductService productService, SharedUtilityService sharedUtilityService)
    {
        _unitOfWork = unitOfWork;
        _productService = productService;
        _sharedUtilityService = sharedUtilityService;
    }



    public async Task<Result<Product>> CreateProductAsync(Guid externalCustomerId, ProductCreateInputDTO productCreateDTO, Worksheet worksheet, CancellationToken cancellationToken = default)
    {
        var productModel = productCreateDTO.ToProductEntity(worksheet);

        var productModelProductOptionVariationListResult = await GenerateProductOptionVariationListAsync(productCreateDTO, cancellationToken);
        if (productModelProductOptionVariationListResult.IsFailed)
        {
            return Result.Fail(productModelProductOptionVariationListResult.Errors);
        }

        productModel.OptionVariations = productModelProductOptionVariationListResult.Value;

        var productPriceAsyncResult = await GenerateProductPriceAsync(productModel, cancellationToken);
        if (productPriceAsyncResult.IsFailed)
        {
            return Result.Fail(productPriceAsyncResult.Errors);
        }

        productModel.Price = productPriceAsyncResult.Value;

        return Result.Ok(productModel);
    }

    private async Task<Result<List<ProductOptionVariation>>> GenerateProductOptionVariationListAsync(ProductCreateInputDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        var populateGeneralConfigurationResult = await _productService.PopulateProductOptionVariationList(productCreateDTO, typeof(ProductCreateInputDTO), cancellationToken);
        if (populateGeneralConfigurationResult.IsFailed)
        {
            return Result.Fail(populateGeneralConfigurationResult.Errors);
        }

        var GeneralConfigurationList = populateGeneralConfigurationResult.Value;


        var kineticsRollerProductOptionVariationListResult = await _productService.PopulateProductOptionVariationList(productCreateDTO.KineticsCellular, typeof(KineticsCellular), cancellationToken);
        if (kineticsRollerProductOptionVariationListResult.IsFailed)
        {
            return Result.Fail(kineticsRollerProductOptionVariationListResult.Errors);
        }

        var kineticsRollerProductOptionVariationList = kineticsRollerProductOptionVariationListResult.Value;

        List<ProductOptionVariation> OptionVariations = [.. GeneralConfigurationList, .. kineticsRollerProductOptionVariationList];

        return Result.Ok(OptionVariations);
    }

    private async Task<Result<decimal>> GenerateProductPriceAsync(Product product, CancellationToken cancellationToken = default)
    {

        var totalPriceProductOptionVariationList = _productService.GetProductOptionVariationListTotalPrice(product.OptionVariations.ToList());

        var generateFabricPriceAsyncResult = await GenerateFabricPriceAsync(product, cancellationToken);

        if (generateFabricPriceAsyncResult.IsFailed)
        {
            return Result.Fail(generateFabricPriceAsyncResult.Errors);
        }

        var fabricPrice = generateFabricPriceAsyncResult.Value;

        return Result.Ok(fabricPrice + totalPriceProductOptionVariationList);
    }

    private async Task<Result<decimal>> GenerateFabricPriceAsync(Product product, CancellationToken cancellationToken = default)
    {

        var fabricOptionVariation = product.OptionVariations.FirstOrDefault(ov => ov.ProductOptionId == FabricOption.ProductOption.ProductOptionId);
        if (fabricOptionVariation is null)
        {
            return Result.Fail(new NotFoundResource("Kinetics Roller Fabric", ""));
        }

        var GetFabricByProductOptionVariationIdResult = await GetFabricByProductOptionVariationId(fabricOptionVariation.ProductOptionVariationId, cancellationToken);
        if (GetFabricByProductOptionVariationIdResult.IsFailed)
        {
            return Result.Fail(GetFabricByProductOptionVariationIdResult.Errors);
        }

        var kineticsRollerFabric = GetFabricByProductOptionVariationIdResult.Value;

        var kineticsRollerFabricOpacityAdjusted = kineticsRollerFabric.Opacity == "BO" ? "LF" : kineticsRollerFabric.Opacity;

        var fabricPrice = await _unitOfWork.FabricPrice.GetFabricPriceByFabricPriceQueryParametersAsync(ProductTypeOption.KineticsRoller.Value, product.Width, product.Height, kineticsRollerFabricOpacityAdjusted, cancellationToken);
        if (fabricPrice is null)
        {
            return Result.Fail(new NotFoundResource("Fabric Price", $"{ProductTypeOption.KineticsRoller.Value} {product.Width}x{product.Height} {kineticsRollerFabric.Opacity}"));
        }

        return Result.Ok(fabricPrice.Price * kineticsRollerFabric.Multiplier);

    }

    public async Task<Result<List<FabricOutputDTO>>> GetFabricListAsync(CancellationToken cancellationToken)
    {
        var fabricList = await _unitOfWork.KineticsRollerFabric.GetAllAsync();
        if (fabricList is null || fabricList.Count == 0)
        {
            return Result.Fail(new Error("Internal Server Issue."));
        }

        var fabricOutputDTOList = fabricList.Select(f => f.ToToFabricOutputDTO()).ToList();

        return Result.Ok(fabricOutputDTOList);
    }

    public Task<Result<FabricOutputDTO>> GetFabricAsync(GetFabricQueryParameters getFabricQueryParameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<Result<KineticsRollerFabric>> GetFabricByProductOptionVariationId(int productOptionVariationId, CancellationToken cancellationToken)
    {
        var kineticsRollerFabric = await _unitOfWork.KineticsRollerFabric.GetFabricByProductOptionVariationIdAsync(productOptionVariationId, cancellationToken);
        if (kineticsRollerFabric is null)
        {
            return Result.Fail(new NotFoundResource("Kinetics Roller Fabirc", ""));

        }

        return Result.Ok(kineticsRollerFabric);

    }

    public Result<ProductEntryOutputDTO> ProductToEntryDTO(Product product, Guid externalWorksheetId)
    {
        if (product is null)
        {
            return Result.Fail(new Error("Input Product is null"));
        }

        var ProductOptionsToDictionaryResult = _productService.ProductOptionsToDictionary(product);
        if (ProductOptionsToDictionaryResult.IsFailed)
        {
            return Result.Fail(ProductOptionsToDictionaryResult.Errors);
        }

        var output = JsonSerializer.Serialize(ProductOptionsToDictionaryResult.Value);
        var x = JsonSerializer.Deserialize<ProductEntryOutputDTO>(output);

        if (x is null)
        {
            return Result.Fail(new Error("Failed to deserialize into product"));
        }


        var productEntryDTO = new ProductEntryOutputDTO
        {
            Id = product.ExternalMapping,
            WorksheetId = externalWorksheetId,
            Price = product.Price,
            Location = product.Location,
            Width = product.Width,
            Height = product.Height,
            Reveal = product.Reveal,
            RemoteNumber = product.RemoteNumber,
            RemoteChannel = product.RemoteChannel,
            InstallHeight = product.InstallHeight,

            FitType = x.FitType,
            FixingTo = x.FixingTo,
            ProductType = x.ProductType,
            Fabric = x.Fabric,
            OperationType = x.OperationType,
            OperationSide = x.OperationSide,
        };


        return Result.Ok(productEntryDTO);
    }


}