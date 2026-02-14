
using FluentResults;
using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.QueryParameters;
using Microsoft.Extensions.Logging;


namespace Lewiss.Pricing.Shared.Services;

public class FabricService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SharedUtilityService _sharedUtilityService;
    private readonly ILogger<FabricService> _logger;
    public FabricService(IUnitOfWork unitOfWork, SharedUtilityService sharedUtilityService, ILogger<FabricService> logger)
    {
        _unitOfWork = unitOfWork;
        _sharedUtilityService = sharedUtilityService;
        _logger = logger;
    }


    // to get rid of switch statements, we will switch to strategy resolver pattern using dictionary
    public virtual async Task<Result<List<IFabricOutputDTO>>> GetFabricsAsync(string productType, CancellationToken cancellationToken = default)
    {

        var queryProductType = _sharedUtilityService.GetProductTypeQueryString(productType);

        var fabricListResult = queryProductType switch
        {
            "kineticscellular" => await GetKineticsCellularFabricListAsync(cancellationToken),
            "kineticsroller" => await GetKineticsRollerFabricListAsync(cancellationToken),
            _ => Result.Fail(new ValidationError("Product Type", productType))
        };

        return fabricListResult;
    }

    // example of where interface will be better
    private async Task<Result<List<IFabricOutputDTO>>> GetKineticsCellularFabricListAsync(CancellationToken cancellationToken = default)
    {
        var fabricList = await _unitOfWork.KineticsCellularFabric.GetAllAsync();

        if (fabricList is null || fabricList.Count == 0)
        {
            return Result.Fail(new Error("Internal Server Issue"));
        }

        List<IFabricOutputDTO> listToReturn = fabricList.Select(f => f.ToKineticsCellularFabricOutputDTO()).ToList<IFabricOutputDTO>();

        return Result.Ok(listToReturn);

    }

    // example of where interface will be better
    private async Task<Result<List<IFabricOutputDTO>>> GetKineticsRollerFabricListAsync(CancellationToken cancellationToken = default)
    {

        var fabricList = await _unitOfWork.KineticsRollerFabric.GetAllAsync();
        if (fabricList is null || fabricList.Count == 0)
        {
            return Result.Fail(new Error("Internal Server Issue"));
        }

        List<IFabricOutputDTO> listToReturn = fabricList.Select(f => f.ToKineticsRollerFabricOutputDTO()).ToList<IFabricOutputDTO>();

        return Result.Ok(listToReturn);
    }
    private async Task<Result<KineticsRollerFabricOutputDTO>> GetKineticsRollerFabricAsync(string? fabric, string colour, string opacity, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fabric))
        {
            return Result.Fail(new ValidationError("Fabric", "null"));
        }

        var kineticsRollerFabric = await _unitOfWork.KineticsRollerFabric.GetFabricAsync(fabric, colour, opacity, cancellationToken);
        if (kineticsRollerFabric is null)
        {
            return Result.Fail(new ValidationError("Kinetics Roller Fabric", $"fabric: {fabric}\ncolour: {colour}\nopacity: {opacity}"));
        }

        var kineticsRollerFabricOutputDTO = kineticsRollerFabric.ToKineticsRollerFabricOutputDTO();

        return Result.Ok(kineticsRollerFabricOutputDTO);
    }

    private async Task<Result<KineticsCellularFabricOutputDTO>> GetKineticsCellularFabricAsync(string colour, string opacity, CancellationToken cancellationToken = default)
    {
        var KineticsCellularFabric = await _unitOfWork.KineticsCellularFabric.GetFabricAsync(colour, opacity, cancellationToken);
        if (KineticsCellularFabric is null)
        {
            return Result.Fail(new ValidationError("Kinetics Cellular Faibrc", $"colour: {colour}\nopacity: {opacity}"));

        }
        return Result.Ok(KineticsCellularFabric.ToKineticsCellularFabricOutputDTO());
    }

    public async Task<Result<FabricPriceOutputDTO>> GetFabricPriceAsync(string productType, GetFabricQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {

        if (string.IsNullOrEmpty(productType))
        {
            return Result.Fail(new ValidationError("Product Type", productType));
        }

        var priceModelResult = await GetFabricPriceModelAsync(productType, queryParameters, cancellationToken);
        if (priceModelResult.IsFailed)
        {
            return Result.Fail(priceModelResult.Errors);
        }

        var priceModel = priceModelResult.Value;
        var fabricMultiplierResult = await GetFabricMultiplier(productType, queryParameters, cancellationToken);
        if (fabricMultiplierResult.IsFailed)
        {
            return Result.Fail(fabricMultiplierResult.Errors);
        }

        var fabricMultiplier = fabricMultiplierResult.Value;
        decimal price = priceModel.Price * fabricMultiplier;
        return Result.Ok(new FabricPriceOutputDTO
        {
            Price = price
        });


    }

    private async Task<Result<FabricPrice>> GetFabricPriceModelAsync(string productType, GetFabricQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (width, height, colour, opacity, fabric) = queryParameters;

        var productTypeAdjustedResult = _sharedUtilityService.GetValidProductOptionTypeString(productType);
        if (productTypeAdjustedResult.IsFailed)
        {
            return Result.Fail(new ValidationError("Product Type", productType));
        }

        var productTypeAdjusted = productTypeAdjustedResult.Value;

        var opacityAdjustedResult = _sharedUtilityService.GetValidFabricOpacityStringForFabricPricing(productType, opacity);
        if (opacityAdjustedResult.IsFailed)
        {
            return Result.Fail(new ValidationError("Opacity", opacity));
        }

        var opacityAdjusted = opacityAdjustedResult.Value;

        var fabricPrice = await _unitOfWork.FabricPrice.GetFabricPriceByFabricPriceQueryParametersAsync(productTypeAdjusted, width, height, opacityAdjusted, cancellationToken);

        if (fabricPrice is null)
        {
            return Result.Fail(new NotFoundResource("Fabric Price", $"{productTypeAdjusted} {width}x{height} {opacity}"));
        }

        return Result.Ok(fabricPrice);

    }


    // again need an interface...
    private async Task<Result<decimal>> GetFabricMultiplier(string productType, GetFabricQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (width, height, colour, opacity, fabric) = queryParameters;

        var productTypeQuery = _sharedUtilityService.GetProductTypeQueryString(productType);

        if (productTypeQuery.Equals("kineticscellular", StringComparison.CurrentCultureIgnoreCase))
        {
            var result = await GetKineticsCellularFabricAsync(colour, opacity, cancellationToken);
            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }
            return Result.Ok(result.Value.Multiplier);
        }
        else if (productTypeQuery.Equals("kineticsroller", StringComparison.CurrentCultureIgnoreCase))
        {
            var result = await GetKineticsRollerFabricAsync(fabric, colour, opacity, cancellationToken);
            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }
            return Result.Ok(result.Value.Multiplier);
        }

        return Result.Fail(new ValidationError("Product Type", productType));
    }

    public async Task<Result<FabricPriceOutputDTO>> GetFabricPriceOutputDTOByProductOptionVariationIdAsync(string productType, int productOptionVariationId, int width, int height, CancellationToken cancellationToken = default)
    {

        var fabricOutputDTOResult = await GetFabricOutputDTOByProductOptionVariationIdAsync(productType, productOptionVariationId, cancellationToken);
        if (fabricOutputDTOResult.IsFailed)
        {
            return Result.Fail(fabricOutputDTOResult.Errors);
        }

        var fabricOutputDTO = fabricOutputDTOResult.Value;

        var productTypeDatabaseValidResult = _sharedUtilityService.GetValidProductOptionTypeString(productType);
        if (productTypeDatabaseValidResult.IsFailed)
        {
            return Result.Fail(productTypeDatabaseValidResult.Errors);
        }

        var productTypeDatabaseValid = productTypeDatabaseValidResult.Value;

        var opacityAdjustedResult = _sharedUtilityService.GetValidFabricOpacityStringForFabricPricing(productType, fabricOutputDTO.Opacity);
        if (opacityAdjustedResult.IsFailed)
        {
            return Result.Fail(new ValidationError("Opacity", fabricOutputDTO.Opacity));
        }

        var opacityAdjusted = opacityAdjustedResult.Value;

        var fabricPrice = await _unitOfWork.FabricPrice.GetFabricPriceByFabricPriceQueryParametersAsync(productTypeDatabaseValid, width, height, opacityAdjusted, cancellationToken);
        if (fabricPrice is null)
        {
            return Result.Fail(new NotFoundResource("Fabric Price", $"{_sharedUtilityService.GetValidProductOptionTypeString(productType)} {width}x{height} {fabricOutputDTO.Opacity}"));
        }

        return Result.Ok(new FabricPriceOutputDTO
        {
            Price = fabricPrice.Price * fabricOutputDTO.Multiplier
        });

    }

    private async Task<Result<IFabricOutputDTO>> GetFabricOutputDTOByProductOptionVariationIdAsync(string productType, int productOptionVariationId, CancellationToken cancellationToken = default)
    {
        var productTypeQuery = _sharedUtilityService.GetProductTypeQueryString(productType);

        // this feels bad
        if (productTypeQuery == _sharedUtilityService.GetProductTypeQueryString(ProductTypeOption.KineticsCellular.Value))
        {
            var kineticsCellularFabric = await _unitOfWork.KineticsCellularFabric.GetFabricByProductOptionVariationIdAsync(productOptionVariationId, cancellationToken);
            if (kineticsCellularFabric is null)
            {
                return Result.Fail(new NotFoundResource("Kinetics Cellular Fabirc", ""));
            }

            // this is why we need interface...
            return Result.Ok(kineticsCellularFabric.ToKineticsCellularFabricOutputDTO() as IFabricOutputDTO);

        }
        else if (productTypeQuery == _sharedUtilityService.GetProductTypeQueryString(ProductTypeOption.KineticsRoller.Value))
        {
            var kineticsRollerFabric = await _unitOfWork.KineticsRollerFabric.GetFabricByProductOptionVariationIdAsync(productOptionVariationId, cancellationToken);
            if (kineticsRollerFabric is null)
            {
                return Result.Fail(new NotFoundResource("Kinetics Roller Fabirc", ""));
            }

            return Result.Ok(kineticsRollerFabric.ToKineticsRollerFabricOutputDTO() as IFabricOutputDTO);

        }

        return Result.Fail(new ValidationError("Product Type", productType));

    }

}