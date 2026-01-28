
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.Product;

namespace Lewiss.Pricing.Shared.Services;

public class PricingService
{
    private readonly IUnitOfWork _unitOfWork;


    public PricingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }



}