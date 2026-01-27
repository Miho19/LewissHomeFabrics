
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.Product;

namespace Lewiss.Pricing.Shared.Services;

public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public virtual async Task<Data.Model.Product?> PopulateProductOptionVariationList(Data.Model.Product product, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {


        return null;
    }

    private async Task<bool> FitTypeOption(Data.Model.Product product, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        var fitType = productCreateDTO.GeneralProductConfigration.FitType;

        if (string.IsNullOrEmpty(fitType))
        {
            return false;
        }


        return true;
    }

}