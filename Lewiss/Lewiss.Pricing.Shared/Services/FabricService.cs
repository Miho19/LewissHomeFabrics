using System.Text.RegularExpressions;
using Lewiss.Pricing.Data.Model.Fabric.Price;
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


    public virtual async Task<List<IFabricOutputDTO>> GetFabricsAsync(string productType, CancellationToken cancellationToken = default)
    {
        try
        {
            var queryProductType = _sharedUtilityService.GetProductTypeQueryString(productType);

            var fabricList = queryProductType switch
            {
                "kineticscellular" => await GetKineticsCellularFabricListAsync(cancellationToken),
                "kineticsroller" => await GetKineticsRollerFabricListAsync(cancellationToken),
                _ => throw new Exception($"Not a valid product type {productType}"),
            };

            return fabricList;

        }
        catch (Exception ex)
        {
            _logger.LogError($"FabricService.GetFabricsAsync exception: ${ex.Message}");
            return [];
        }

    }

    private async Task<List<IFabricOutputDTO>> GetKineticsCellularFabricListAsync(CancellationToken cancellationToken = default)
    {
        var fabricList = await _unitOfWork.KineticsCellularFabric.GetAllAsync();

        if (fabricList is null || fabricList.Count == 0)
        {
            throw new Exception("Could not retrieve all Kinetic Cellular fabrics");
        }

        List<IFabricOutputDTO> listToReturn = fabricList.Select(f => f.ToKineticsCellularFabricOutputDTO()).ToList<IFabricOutputDTO>();

        return listToReturn;

    }


    private async Task<List<IFabricOutputDTO>> GetKineticsRollerFabricListAsync(CancellationToken cancellationToken = default)
    {

        var fabricList = await _unitOfWork.KineticsRollerFabric.GetAllAsync();
        if (fabricList is null || fabricList.Count == 0)
        {
            throw new Exception("Could not retrieve all Kinetic Roller fabrics");
        }

        List<IFabricOutputDTO> listToReturn = fabricList.Select(f => f.ToKineticsRollerFabricOutputDTO()).ToList<IFabricOutputDTO>();

        return listToReturn;
    }

    private async Task<IFabricOutputDTO> GetKineticsRollerFabricAsync(string? fabric, string colour, string opacity, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fabric))
        {
            throw new Exception("Kinetics Roller fabrics required a fabric to query for");
        }

        var kineticsRollerFabric = await _unitOfWork.KineticsRollerFabric.GetFabricAsync(fabric, colour, opacity, cancellationToken);
        if (kineticsRollerFabric is null)
        {
            throw new Exception($"Could not find Kinetics Roller fabric for fabric {fabric} colour {colour} opacity {opacity}");
        }

        return kineticsRollerFabric.ToKineticsRollerFabricOutputDTO();
    }

    private async Task<IFabricOutputDTO> GetKineticsCellularFabricAsync(string colour, string opacity, CancellationToken cancellationToken = default)
    {
        var KineticsCellularFabric = await _unitOfWork.KineticsCellularFabric.GetFabricAsync(colour, opacity, cancellationToken);
        if (KineticsCellularFabric is null)
        {
            throw new Exception($"Could not find Kinetics Cellular fabric for colour: {colour} opacity: {opacity}");
        }
        return KineticsCellularFabric.ToKineticsCellularFabricOutputDTO();
    }

    public async Task<FabricPriceOutputDTO?> GetFabricPriceAsync(string productType, GetFabricPriceQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(productType))
            {
                throw new Exception("Product Type is null");
            }

            var priceModel = await GetFabricPriceModelAsync(productType, queryParameters, cancellationToken);
            var fabricMultiplier = await GetFabricMultiplier(productType, queryParameters, cancellationToken);

            // just to be explicit
            decimal price = priceModel.Price * fabricMultiplier;
            return new FabricPriceOutputDTO
            {
                Price = price
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"FabricService.GetFabricPriceAsync exception: {ex.Message}");
            return null;
        }

    }

    private async Task<FabricPrice> GetFabricPriceModelAsync(string productType, GetFabricPriceQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (width, height, colour, opacity, fabric) = queryParameters;

        var productTypeAdjusted = _sharedUtilityService.GetValidProductOptionTypeString(productType);

        var fabricPrice = await _unitOfWork.FabricPrice.GetFabricPriceByFabricPriceQueryParametersAsync(productTypeAdjusted, width, height, opacity, cancellationToken);

        if (fabricPrice is null)
        {
            throw new Exception($"Failed to retrieve Fabric Price Model for {productTypeAdjusted} {width}x{height} {opacity}");
        }

        return fabricPrice;

    }

    private async Task<decimal> GetFabricMultiplier(string productType, GetFabricPriceQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (width, height, colour, opacity, fabric) = queryParameters;

        var productTypeQuery = _sharedUtilityService.GetProductTypeQueryString(productType);

        var fabricDTO = productTypeQuery switch
        {
            "kineticscellular" => await GetKineticsCellularFabricAsync(colour, opacity, cancellationToken),
            "kineticsroller" => await GetKineticsRollerFabricAsync(fabric, colour, opacity, cancellationToken),
            _ => throw new Exception("Invalid Product Type"),
        };

        return fabricDTO.Multiplier;
    }

    public async Task<FabricPriceOutputDTO?> GetFabricPriceOutputDTOByProductOptionVariationIdAsync(int productOptionVariationId, CancellationToken cancellationToken = default)
    {
        try
        {

            return new FabricPriceOutputDTO
            {
                Price = default
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"FabricService.GetFabricPriceOutputDTOByProductOptionVariationIdAsync exception: {ex.Message}");
            return null;
        }
    }

}