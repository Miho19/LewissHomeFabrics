using System.Reflection;
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

        Type variationProductType = typeof(VariationProductConfiguration);
        var variationProductTypeProperties = variationProductType.GetProperties();

        foreach (var property in variationProductTypeProperties)
        {
            var productOption = await _unitOfWork.ProductOption.GetProductOptionByNameAsync(property.Name, cancellationToken);
            if (productOption is null)
            {
                return null;
            }

            var propertyValue = (string)property.GetValue(productCreateDTO.VariationProductConfiguration)!;
            var productVariation = productOption.ProductOptionVariation.FirstOrDefault(pv => pv.Value == propertyValue);
            if (productVariation is null)
            {
                return null;
            }

            product.OptionVariations.Add(productVariation);

        }

        return product;
    }



}

