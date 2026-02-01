using System.Text.RegularExpressions;
using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Shared.FabricDTO;
using Lewiss.Pricing.Shared.QueryParameters;


namespace Lewiss.Pricing.Shared.Services;

public class FabricService
{
    private readonly IUnitOfWork _unitOfWork;
    public FabricService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public virtual async Task<List<IFabricOutputDTO>> GetFabricsAsync(string fabricType, CancellationToken cancellationToken = default)
    {
        var query = Regex.Replace(fabricType, @"\s+", String.Empty).ToLower();


        var fabricList = query switch
        {
            "kineticscellular" => await GetKineticsCellularFabricListAsync(cancellationToken),
            "kineticsroller" => await GetKineticsRollerFabricListAsync(cancellationToken),
            _ => [],
        };

        return fabricList;
    }

    private async Task<List<IFabricOutputDTO>> GetKineticsCellularFabricListAsync(CancellationToken cancellationToken = default)
    {
        var fabricList = await _unitOfWork.KineticsCellularFabric.GetAllAsync();

        if (fabricList is null || fabricList.Count == 0)
        {
            return [];
        }

        List<IFabricOutputDTO> listToReturn = fabricList.Select(f => f.ToKineticsCellularFabricOutputDTO()).ToList<IFabricOutputDTO>();

        return listToReturn;

    }


    private async Task<List<IFabricOutputDTO>> GetKineticsRollerFabricListAsync(CancellationToken cancellationToken = default)
    {

        var fabricList = await _unitOfWork.KineticsRollerFabric.GetAllAsync();
        if (fabricList is null || fabricList.Count == 0)
        {
            return [];
        }

        List<IFabricOutputDTO> listToReturn = fabricList.Select(f => f.ToKineticsRollerFabricOutputDTO()).ToList<IFabricOutputDTO>();

        return listToReturn;
    }

    private async Task<IFabricOutputDTO?> GetKineticsRollerFabricAsync(string? fabric, string colour, string opacity, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(fabric))
        {
            return null;
        }

        var kineticsRollerFabric = await _unitOfWork.KineticsRollerFabric.GetFabricAsync(fabric, colour, opacity, cancellationToken);
        if (kineticsRollerFabric is null)
        {
            return null;
        }

        return kineticsRollerFabric.ToKineticsRollerFabricOutputDTO();
    }

    private async Task<IFabricOutputDTO?> GetKineticsCellularFabricAsync(string colour, string opacity, CancellationToken cancellationToken = default)
    {
        var KineticsCellularFabric = await _unitOfWork.KineticsCellularFabric.GetFabricAsync(colour, opacity, cancellationToken);
        if (KineticsCellularFabric is null)
        {
            return null;
        }
        return KineticsCellularFabric.ToKineticsCellularFabricOutputDTO();
    }

    public async Task<FabricPriceOutputDTO?> GetFabricPriceAsync(string productType, GetFabricPriceQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(productType))
        {
            return null;
        }

        var priceModel = await GetFabricPriceModelAsync(productType, queryParameters, cancellationToken);
        if (priceModel is null)
        {
            return null;
        }

        var fabricMultiplier = await GetFabricMultiplier(productType, queryParameters, cancellationToken);
        if (fabricMultiplier == default)
        {
            return null;
        }

        // just to be explicit
        decimal price = priceModel.Price * fabricMultiplier;



        return new FabricPriceOutputDTO
        {
            Price = price
        };
    }

    private async Task<FabricPrice?> GetFabricPriceModelAsync(string productType, GetFabricPriceQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (width, height, colour, opacity, fabric) = queryParameters;

        var query = Regex.Replace(productType, @"\s+", String.Empty).ToLower();

        var productTypeAdjusted = query switch
        {
            "kineticscellular" => "Kinetics Cellular",
            "kineticsroller" => "Kinetics Roller",
            _ => null,
        };

        if (string.IsNullOrEmpty(productTypeAdjusted))
        {
            return null;
        }

        return await _unitOfWork.FabricPrice.GetFabricPriceByFabricPriceQueryParametersAsync(productTypeAdjusted, width, height, opacity, cancellationToken);

    }

    private async Task<decimal> GetFabricMultiplier(string productType, GetFabricPriceQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (width, height, colour, opacity, fabric) = queryParameters;

        var query = Regex.Replace(productType, @"\s+", String.Empty).ToLower();

        var fabricDTO = query switch
        {
            "kineticscellular" => await GetKineticsCellularFabricAsync(colour, opacity, cancellationToken),
            "kineticsroller" => await GetKineticsRollerFabricAsync(fabric, colour, opacity, cancellationToken),
            _ => null,
        };

        if (fabricDTO is null)
        {
            return default;
        }


        return fabricDTO.Multiplier;
    }

}