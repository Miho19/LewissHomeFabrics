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



    public Task<Result<Product>> CreateProductAsync(Guid externalCustomerId, ProductCreateInputDTO productCreateDTO, Worksheet worksheet, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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

}