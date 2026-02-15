
using FluentResults;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.ProductStrategy;
using Microsoft.Extensions.Logging;


namespace Lewiss.Pricing.Shared.Services;

public class FabricService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SharedUtilityService _sharedUtilityService;
    private readonly ProductStrategyResolver _productStrategyResolver;

    private readonly ILogger<FabricService> _logger;
    public FabricService(IUnitOfWork unitOfWork, SharedUtilityService sharedUtilityService, ProductStrategyResolver productStrategyResolver, ILogger<FabricService> logger)
    {
        _unitOfWork = unitOfWork;
        _sharedUtilityService = sharedUtilityService;
        _productStrategyResolver = productStrategyResolver;
        _logger = logger;
    }


    public virtual async Task<Result<List<FabricOutputDTO>>> GetFabricsAsync(string productType, CancellationToken cancellationToken = default)
    {

        var productStategyResolverResult = _productStrategyResolver.GetProductStrategyByProductTypeString(productType);
        if (productStategyResolverResult.IsFailed)
        {
            return Result.Fail(productStategyResolverResult.Errors);
        }

        var productStrategy = productStategyResolverResult.Value;

        var fabricListResult = await productStrategy.GetFabricListAsync(cancellationToken);
        if (fabricListResult.IsFailed)
        {
            return Result.Fail(fabricListResult.Errors);
        }

        return Result.Ok(fabricListResult.Value);
    }

}