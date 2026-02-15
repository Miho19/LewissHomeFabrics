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

public class KineticsCellularProductStrategy : IProductStrategy
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductService _productService;

    private readonly SharedUtilityService _sharedUtilityService;

    public string ProductType => ProductTypeOption.KineticsCellular.Value;


    public KineticsCellularProductStrategy(IUnitOfWork unitOfWork, ProductService productService, SharedUtilityService sharedUtilityService)
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


        var kineticsCellularProductOptionVariationListResult = await _productService.PopulateProductOptionVariationList(productCreateDTO.KineticsCellular, typeof(KineticsCellular), cancellationToken);
        if (kineticsCellularProductOptionVariationListResult.IsFailed)
        {
            return Result.Fail(kineticsCellularProductOptionVariationListResult.Errors);
        }

        var kineticsCellularProductOptionVariationList = kineticsCellularProductOptionVariationListResult.Value;

        List<ProductOptionVariation> OptionVariations = [.. GeneralConfigurationList, .. kineticsCellularProductOptionVariationList];

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
            return Result.Fail(new NotFoundResource("Kinetics Cellular Fabric", ""));
        }

        var GetFabricByProductOptionVariationIdResult = await GetFabricByProductOptionVariationId(fabricOptionVariation.ProductOptionVariationId, cancellationToken);
        if (GetFabricByProductOptionVariationIdResult.IsFailed)
        {
            return Result.Fail(GetFabricByProductOptionVariationIdResult.Errors);
        }

        var kineticsCellularFabric = GetFabricByProductOptionVariationIdResult.Value;

        var fabricPrice = await _unitOfWork.FabricPrice.GetFabricPriceByFabricPriceQueryParametersAsync(ProductTypeOption.KineticsCellular.Value, product.Width, product.Height, kineticsCellularFabric.Opacity, cancellationToken);
        if (fabricPrice is null)
        {
            return Result.Fail(new NotFoundResource("Fabric Price", $"{ProductTypeOption.KineticsCellular.Value} {product.Width}x{product.Height} {kineticsCellularFabric.Opacity}"));
        }

        return Result.Ok(fabricPrice.Price * kineticsCellularFabric.Multiplier);

    }

    public async Task<Result<List<FabricOutputDTO>>> GetFabricListAsync(CancellationToken cancellationToken)
    {
        var fabricList = await _unitOfWork.KineticsCellularFabric.GetAllAsync();
        if (fabricList is null || fabricList.Count == 0)
        {
            return Result.Fail(new Error("Internal Server Issue."));
        }

        var fabricOutputDTOList = fabricList.Select(f => f.ToFabricOutputDTO()).ToList();

        return Result.Ok(fabricOutputDTOList);
    }

    public Task<Result<FabricOutputDTO>> GetFabricAsync(GetFabricQueryParameters getFabricQueryParameters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    private async Task<Result<KineticsCellularFabric>> GetFabricByProductOptionVariationId(int productOptionVariationId, CancellationToken cancellationToken)
    {
        var kineticsCellularFabric = await _unitOfWork.KineticsCellularFabric.GetFabricByProductOptionVariationIdAsync(productOptionVariationId, cancellationToken);
        if (kineticsCellularFabric is null)
        {
            return Result.Fail(new NotFoundResource("Kinetics Cellular Fabirc", ""));
        }

        return Result.Ok(kineticsCellularFabric);

    }


}