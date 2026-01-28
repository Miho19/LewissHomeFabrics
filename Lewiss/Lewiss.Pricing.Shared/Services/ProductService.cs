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

    public virtual async Task<ProductEntryDTO?> CreateProductAsync(Guid externalCustomerId, Guid externalWorksheetId, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        // This will eventually be replaced by function in PricingService and Result pattern; currently this check is duplicated
        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(externalCustomerId, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var worksheet = await _unitOfWork.Worksheet.GetWorksheetByExternalIdAsync(externalWorksheetId, cancellationToken);
        if (worksheet is null)
        {
            return null;
        }

        if (worksheet.CustomerId != customer.CustomerId)
        {
            return null;
        }

        var product = productCreateDTO.ToProductEntity(worksheet);

        product = await PopulateProductOptionVariationList(product, productCreateDTO, cancellationToken);
        if (product is null)
        {
            return null;
        }

        await _unitOfWork.Product.AddAsync(product);
        await _unitOfWork.CommitAsync();

        var productEntryDTO = product.ToProductEntryDTO(productCreateDTO);

        return productEntryDTO;
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

