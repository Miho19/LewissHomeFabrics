using System.Text.RegularExpressions;
using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Data.OptionData;
using Lewiss.Pricing.Shared.Error;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public virtual async Task<List<IFabricOutputDTO>> GetFabricsAsync(string productType, CancellationToken cancellationToken = default)
    {

        var queryProductType = _sharedUtilityService.GetProductTypeQueryString(productType);

        var fabricList = queryProductType switch
        {
            "kineticscellular" => await GetKineticsCellularFabricListAsync(cancellationToken),
            "kineticsroller" => await GetKineticsRollerFabricListAsync(cancellationToken),
            _ => throw new InvalidQueryParameterException("Product type is invalid")
        };

        return fabricList;



    }

    private async Task<List<IFabricOutputDTO>> GetKineticsCellularFabricListAsync(CancellationToken cancellationToken = default)
    {
        var fabricList = await _unitOfWork.KineticsCellularFabric.GetAllAsync();

        if (fabricList is null || fabricList.Count == 0)
        {
            throw new BaseException(StatusCodes.Status500InternalServerError, "Could not retrieve Kinetic Cellular fabrics");
        }

        List<IFabricOutputDTO> listToReturn = fabricList.Select(f => f.ToKineticsCellularFabricOutputDTO()).ToList<IFabricOutputDTO>();

        return listToReturn;

    }


    private async Task<List<IFabricOutputDTO>> GetKineticsRollerFabricListAsync(CancellationToken cancellationToken = default)
    {

        var fabricList = await _unitOfWork.KineticsRollerFabric.GetAllAsync();
        if (fabricList is null || fabricList.Count == 0)
        {
            throw new BaseException(StatusCodes.Status500InternalServerError, "Could not retrieve Kinetic Roller fabrics");
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

    public async Task<FabricPriceOutputDTO> GetFabricPriceAsync(string productType, GetFabricPriceQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {

        if (string.IsNullOrEmpty(productType))
        {
            throw new InvalidQueryParameterException("Product type is invalid");
        }

        var priceModel = await GetFabricPriceModelAsync(productType, queryParameters, cancellationToken);
        var fabricMultiplier = await GetFabricMultiplier(productType, queryParameters, cancellationToken);


        decimal price = priceModel.Price * fabricMultiplier;
        return new FabricPriceOutputDTO
        {
            Price = price
        };


    }

    private async Task<FabricPrice> GetFabricPriceModelAsync(string productType, GetFabricPriceQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (width, height, colour, opacity, fabric) = queryParameters;

        var productTypeAdjusted = _sharedUtilityService.GetValidProductOptionTypeString(productType);
        var opacityAdjusted = _sharedUtilityService.GetValidFabricOpacityStringForFabricPricing(productType, opacity);
        var fabricPrice = await _unitOfWork.FabricPrice.GetFabricPriceByFabricPriceQueryParametersAsync(productTypeAdjusted, width, height, opacityAdjusted, cancellationToken);

        if (fabricPrice is null)
        {
            throw new NotFoundException($"Price not found for {productTypeAdjusted} {width}x{height} {opacity}");
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
            _ => throw new InvalidQueryParameterException("Product type is invalid"),
        };

        return fabricDTO.Multiplier;
    }

    public async Task<FabricPriceOutputDTO?> GetFabricPriceOutputDTOByProductOptionVariationIdAsync(string productType, int productOptionVariationId, int width, int height, CancellationToken cancellationToken = default)
    {

        var fabricOutputDTO = await GetFabricOutputDTOByProductOptionVariationIdAsync(productType, productOptionVariationId, cancellationToken);

        var productTypeDatabaseValid = _sharedUtilityService.GetValidProductOptionTypeString(productType);
        var opacityAdjusted = _sharedUtilityService.GetValidFabricOpacityStringForFabricPricing(productType, fabricOutputDTO.Opacity);
        var fabricPrice = await _unitOfWork.FabricPrice.GetFabricPriceByFabricPriceQueryParametersAsync(productTypeDatabaseValid, width, height, opacityAdjusted, cancellationToken);
        if (fabricPrice is null)
        {
            return null;
        }

        return new FabricPriceOutputDTO
        {
            Price = fabricPrice.Price * fabricOutputDTO.Multiplier
        };

    }

    private async Task<IFabricOutputDTO> GetFabricOutputDTOByProductOptionVariationIdAsync(string productType, int productOptionVariationId, CancellationToken cancellationToken = default)
    {
        var productTypeQuery = _sharedUtilityService.GetProductTypeQueryString(productType);

        // this feels bad
        if (productTypeQuery == _sharedUtilityService.GetProductTypeQueryString(ProductTypeOption.KineticsCellular.Value))
        {
            var kineticsCellularFabric = await _unitOfWork.KineticsCellularFabric.GetFabricByProductOptionVariationIdAsync(productOptionVariationId, cancellationToken);
            if (kineticsCellularFabric is null)
            {
                throw new NotFoundException($"Failed to retrieve Kinetics Cellular fabric");
            }

            return kineticsCellularFabric.ToKineticsCellularFabricOutputDTO();

        }
        else if (productTypeQuery == _sharedUtilityService.GetProductTypeQueryString(ProductTypeOption.KineticsRoller.Value))
        {
            var kineticsRollerFabric = await _unitOfWork.KineticsRollerFabric.GetFabricByProductOptionVariationIdAsync(productOptionVariationId, cancellationToken);
            if (kineticsRollerFabric is null)
            {
                throw new Exception($"Failed to retrieve Kinetics Roller fabric");
            }

            return kineticsRollerFabric.ToKineticsRollerFabricOutputDTO();

        }
        else
        {
            throw new InvalidQueryParameterException("Product type is invalid");
        }

    }

}