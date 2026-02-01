using System.Text.Json;
using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Shared.ProductDTO;

public static class ProductExtensions
{
    public static Product ToProductEntity(this ProductCreateInputDTO productCreateDTO, Worksheet worksheet)
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

    public static ProductEntryOutputDTO ToProductEntryDTO(this Product product, ProductCreateInputDTO productCreateDTO)
    {
        if (product is null || productCreateDTO is null)
        {
            throw new Exception("Inputs are null");
        }

        return new ProductEntryOutputDTO
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

    public static ProductEntryOutputDTO ToProductEntryDTO(this Product product, Guid externalWorksheetId)
    {
        if (product is null || externalWorksheetId == Guid.Empty)
        {
            throw new Exception("Inputs are null");
        }

        var optionsDictionary = ProductOptionsToDictionary(product);
        if (optionsDictionary.Count == 0)
        {
            throw new Exception("Product options variations are empty");
        }

        var fixedConfiguration = ProductOptionsDictionaryToFixedConfiguration(optionsDictionary);
        if (fixedConfiguration is null)
        {
            throw new Exception("Fixed Configuration was not populated");
        }

        var variableConfiguration = ProductToVariableConfiguration(product);
        if (variableConfiguration is null)
        {
            throw new Exception("Variable Configuration was not populated");
        }

        var productEntryDTO = new ProductEntryOutputDTO
        {
            Id = product.ExternalMapping,
            WorksheetId = externalWorksheetId,
            Price = product.Price,
            VariableConfiguration = variableConfiguration,
            FixedConfiguration = fixedConfiguration,
        };

        productEntryDTO = ProductEntryDTOPopulateSpecificConfiguration(productEntryDTO, optionsDictionary);
        if (productEntryDTO is null)
        {
            throw new Exception("Specific Configuration was not populated");
        }



        return productEntryDTO;
    }

    private static Dictionary<string, object> ProductOptionsToDictionary(Product product)
    {
        var optionsDictionary = new Dictionary<string, object>();

        foreach (var op in product.OptionVariations)
        {
            if (op.ProductOption is null)
                continue;

            optionsDictionary.Add(op.ProductOption.Name, op.Value);
        }

        return optionsDictionary;
    }

    private static FixedConfiguration? ProductOptionsDictionaryToFixedConfiguration(Dictionary<string, object> optionsDictionary)
    {
        var optionsDictionaryJsonString = JsonSerializer.Serialize(optionsDictionary);
        var fixedConfiguration = JsonSerializer.Deserialize<FixedConfiguration>(optionsDictionaryJsonString);

        return fixedConfiguration;
    }

    private static VariableConfiguration ProductToVariableConfiguration(Product product)
    {
        var variableConfiguration = new VariableConfiguration()
        {
            Location = product.Location,
            Width = product.Width,
            Height = product.Height,
            Reveal = product.Reveal,
            RemoteNumber = product.RemoteNumber,
            RemoteChannel = product.RemoteChannel,
            InstallHeight = product.InstallHeight,
        };

        return variableConfiguration;
    }

    private static ProductEntryOutputDTO? ProductEntryDTOPopulateSpecificConfiguration(ProductEntryOutputDTO productEntryDTO, Dictionary<string, object> optionsDictionary)
    {

        var productType = productEntryDTO.FixedConfiguration.ProductType.ToUpper();
        productType = String.Concat(productType.Where(c => !Char.IsWhiteSpace(c)));


        var result = productType switch
        {
            "KINETICSCELLULAR" => ProductEntryDTOPopulateKineticsCellular(productEntryDTO, optionsDictionary),
            "KINETICSROLLER" => ProductEntryDTOPopulateKineticsRoller(productEntryDTO, optionsDictionary),
            _ => null,
        };

        if (result is null)
        {
            return null;
        }

        return result;
    }

    private static ProductEntryOutputDTO? ProductEntryDTOPopulateKineticsCellular(ProductEntryOutputDTO productEntryDTO, Dictionary<string, object> optionsDictionary)
    {
        var optionsDictionaryJsonString = JsonSerializer.Serialize(optionsDictionary);
        var kineticsCellular = JsonSerializer.Deserialize<KineticsCellular>(optionsDictionaryJsonString);
        if (kineticsCellular is null)
        {
            return null;
        }

        productEntryDTO.KineticsCellular = kineticsCellular;
        return productEntryDTO;
    }
    private static ProductEntryOutputDTO? ProductEntryDTOPopulateKineticsRoller(ProductEntryOutputDTO productEntryDTO, Dictionary<string, object> optionsDictionary)
    {
        var optionsDictionaryJsonString = JsonSerializer.Serialize(optionsDictionary);
        var kineticsRoller = JsonSerializer.Deserialize<KineticsRoller>(optionsDictionaryJsonString);
        if (kineticsRoller is null)
        {
            return null;
        }
        productEntryDTO.KineticsRoller = kineticsRoller;
        return productEntryDTO;
    }

}