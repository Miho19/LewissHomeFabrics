using Lewiss.Pricing.Shared.Product;

public class FabricService
{
    private readonly IUnitOfWork _unitOfWork;
    public FabricService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<List<IFabricDTO>> GetFabricsAsync(string fabricType, CancellationToken cancellationToken = default)
    {
        var fabricList = fabricType switch
        {
            "Kinetics Cellular" => await GetKineticsCellularFabricListAsync(cancellationToken),
            "Kinetics Roller" => await GetKineticsRollerFabricListAsync(cancellationToken),
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

        return (List<IFabricDTO>)fabricList.Select(f => f.ToKineticsCellularFabricDTO());

    }


    private async Task<List<IFabricDTO>> GetKineticsRollerFabricListAsync(CancellationToken cancellationToken = default)
    {

        var fabricList = await _unitOfWork.KineticsRollerFabric.GetAllAsync();

        if (fabricList is null || fabricList.Count == 0)
        {
            return [];
        }

        return (List<IFabricDTO>)fabricList.Select(f => f.ToKineticsRollerFabricDTO());
    }

}