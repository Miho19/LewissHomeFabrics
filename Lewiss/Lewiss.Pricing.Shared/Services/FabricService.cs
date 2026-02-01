using System.Text.RegularExpressions;
using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Shared.Product;
using Lewiss.Pricing.Shared.QueryParameters;

public class FabricService
{
    private readonly IUnitOfWork _unitOfWork;
    public FabricService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public virtual async Task<List<IFabricDTO>> GetFabricsAsync(string fabricType, CancellationToken cancellationToken = default)
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

    private async Task<List<IFabricDTO>> GetKineticsCellularFabricListAsync(CancellationToken cancellationToken = default)
    {
        var fabricList = await _unitOfWork.KineticsCellularFabric.GetAllAsync();

        if (fabricList is null || fabricList.Count == 0)
        {
            return [];
        }

        List<IFabricDTO> listToReturn = fabricList.Select(f => f.ToKineticsCellularFabricDTO()).ToList<IFabricDTO>();

        return listToReturn;

    }


    private async Task<List<IFabricDTO>> GetKineticsRollerFabricListAsync(CancellationToken cancellationToken = default)
    {

        var fabricList = await _unitOfWork.KineticsRollerFabric.GetAllAsync();
        if (fabricList is null || fabricList.Count == 0)
        {
            return [];
        }

        List<IFabricDTO> listToReturn = fabricList.Select(f => f.ToKineticsRollerFabricDTO()).ToList<IFabricDTO>();

        return listToReturn;
    }

    private async Task<IFabricDTO?> GetKineticsRollerFabricAsync(string? fabric, string colour, string opacity, CancellationToken cancellationToken = default)
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

        return kineticsRollerFabric.ToKineticsRollerFabricDTO();
    }

    private async Task<IFabricDTO?> GetKineticsCellularFabricAsync(string colour, string opacity, CancellationToken cancellationToken = default)
    {
        var KineticsCellularFabric = await _unitOfWork.KineticsCellularFabric.GetFabricAsync(colour, opacity, cancellationToken);
        if (KineticsCellularFabric is null)
        {
            return null;
        }
        return KineticsCellularFabric.ToKineticsCellularFabricDTO();
    }

    public async Task<decimal> GetFabricPriceAsync(string productType, GetFabricPriceQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(productType))
        {
            return default;
        }

        var priceModel = await GetFabricPriceModelAsync(productType, queryParameters, cancellationToken);
        if (priceModel is null)
        {
            return default;
        }

        var fabricMultiplier = await GetFabricMultiplier(productType, queryParameters, cancellationToken);
        if (fabricMultiplier == default)
        {
            return default;
        }

        // just to be explicit
        decimal price = priceModel.Price * fabricMultiplier;

        return price;
    }

    private async Task<FabricPrice?> GetFabricPriceModelAsync(string productType, GetFabricPriceQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (width, height, colour, opacity, fabric) = queryParameters;

        return await _unitOfWork.FabricPrice.GetFabricPriceByFabricPriceQueryParametersAsync(productType, width, height, opacity, cancellationToken);

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