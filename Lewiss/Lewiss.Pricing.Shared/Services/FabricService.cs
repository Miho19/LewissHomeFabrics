public class FabricService
{
    private readonly IUnitOfWork _unitOfWork;
    public FabricService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


}