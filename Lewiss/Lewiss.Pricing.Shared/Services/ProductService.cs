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

    public virtual async Task<ProductEntryDTO?> CreateProductAsync(Guid externalWorksheetId, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {

        var worksheet = await _unitOfWork.Worksheet.GetWorksheetByExternalIdAsync(externalWorksheetId, cancellationToken);
        if (worksheet is null)
        {
            return null;
        }

        var generalConfiguration = productCreateDTO.GeneralProductConfigration;

        var product = new Data.Model.Product
        {
            ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
            Price = generalConfiguration.Price,
            Location = generalConfiguration.Location,
            Width = generalConfiguration.Width,
            Height = generalConfiguration.Height,
            Reveal = generalConfiguration.Reveal,
            AboveHeightConstraint = generalConfiguration.AboveHeightConstraint,
            RemoteNumber = generalConfiguration.RemoteNumber,
            RemoteChannel = generalConfiguration.RemoteChannel,
            WorksheetId = worksheet.WorksheetId,
            Worksheet = worksheet
        };

        product = await PopulateProductOptionVariationList(product, productCreateDTO, cancellationToken);
        if (product is null)
        {
            return null;
        }

        await _unitOfWork.Product.AddAsync(product);
        await _unitOfWork.CommitAsync();

        var productEntryDTO = new ProductEntryDTO
        {
            Id = product.ExternalMapping,
            WorksheetId = externalWorksheetId,
            Configuration = productCreateDTO.Configuration,
            GeneralConfiguration = productCreateDTO.GeneralProductConfigration,
            VariationProductConfiguration = productCreateDTO.VariationProductConfiguration
        };

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

