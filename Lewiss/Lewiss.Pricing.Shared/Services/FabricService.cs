using System.Text.RegularExpressions;
using Lewiss.Pricing.Shared.Product;

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

}