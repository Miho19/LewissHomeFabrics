using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.Product;

public static class ProductExtensions
{
    public static Product ToProductEntity(this ProductCreateDTO productCreateDTO, Worksheet worksheet)
    {
        if (productCreateDTO is null || worksheet is null)
        {
            throw new Exception("Inputs are null");
        }

        var generalConfiguration = productCreateDTO.GeneralProductConfigration;

        return new Product
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
    }

    public static ProductEntryDTO ToProductEntryDTO(this Product product, ProductCreateDTO productCreateDTO)
    {
        if (product is null || productCreateDTO is null)
        {
            throw new Exception("Inputs are null");
        }

        return new ProductEntryDTO
        {
            Id = product.ExternalMapping,
            WorksheetId = productCreateDTO.WorksheetId,
            Configuration = productCreateDTO.Configuration,
            GeneralConfiguration = productCreateDTO.GeneralProductConfigration,
            VariationProductConfiguration = productCreateDTO.VariationProductConfiguration
        };
    }
}