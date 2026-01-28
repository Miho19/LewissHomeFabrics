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

        var variableConfiguration = productCreateDTO.VariableConfiguration;

        return new Product
        {
            ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
            InstallHeight = variableConfiguration.InstallHeight,
            Location = variableConfiguration.Location,
            Width = variableConfiguration.Width,
            Height = variableConfiguration.Height,
            Reveal = variableConfiguration.Reveal,
            RemoteNumber = variableConfiguration.RemoteNumber,
            RemoteChannel = variableConfiguration.RemoteChannel,
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
            Price = product.Price,
            VariableConfiguration = productCreateDTO.VariableConfiguration,
            FixedConfiguration = productCreateDTO.FixedConfiguration,
            KineticsCellular = productCreateDTO.KineticsCellular,
            KineticsRoller = productCreateDTO.KineticsRoller,
        };
    }
}