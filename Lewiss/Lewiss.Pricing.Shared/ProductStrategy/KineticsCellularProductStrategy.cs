using FluentResults;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Data.Model.Fabric.Type;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.ProductStrategy;
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

        var kineticsCellularroductOptionVariationList = kineticsCellularProductOptionVariationListResult.Value;
        productModel.OptionVariations = [.. GeneralConfigurationList, .. kineticsCellularroductOptionVariationList];

        var totalPriceProductOptionVariationList = _productService.GetProductOptionVariationListTotalPrice(productModel.OptionVariations.ToList());


        var fabricPriceResult = await GetProductFabricPriceOutputDTO(product.OptionVariations.ToList(), product.Width, product.Height, cancellationToken);
        if (fabricPriceResult.IsFailed)
        {
            return Result.Fail(fabricPriceResult.Errors);
        }

        var fabricPrice = fabricPriceResult.Value;


        product.Price = totalPriceProductOptionVariationList + fabricPrice.Price;



        return Result.Ok();
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
}