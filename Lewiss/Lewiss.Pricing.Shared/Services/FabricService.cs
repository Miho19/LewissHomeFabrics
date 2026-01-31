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

        return [];
    }


}