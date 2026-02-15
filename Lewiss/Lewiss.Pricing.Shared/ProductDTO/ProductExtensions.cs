using System.Text.Json;
using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Shared.ProductDTO;

public static class ProductExtensions
{
    public static Product ToProductEntity(this ProductCreateInputDTO productCreateDTO, Worksheet worksheet)
    {

        return new Product
        {
            ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
            Price = 0.00m,
            Location = productCreateDTO.Location,
            Width = productCreateDTO.Width,
            Height = productCreateDTO.Height,
            Reveal = productCreateDTO.Reveal,
            InstallHeight = productCreateDTO.InstallHeight,
            RemoteNumber = productCreateDTO.RemoteNumber,
            RemoteChannel = productCreateDTO.RemoteChannel,
            WorksheetId = worksheet.WorksheetId,
            Worksheet = worksheet
        };
    }

    public static ProductEntryOutputDTO ToProductEntryDTO(this Product product, ProductCreateInputDTO productCreateDTO)
    {

        return new ProductEntryOutputDTO
        {
            Id = product.ExternalMapping,
            WorksheetId = productCreateDTO.WorksheetId,
            Price = product.Price,
            Location = product.Location,
            Width = product.Width,
            Height = product.Height,
            Reveal = product.Reveal,
            RemoteNumber = product.RemoteNumber,
            RemoteChannel = product.RemoteChannel,
            InstallHeight = product.InstallHeight,
            FitType = productCreateDTO.FitType,
            FixingTo = productCreateDTO.FixingTo,
            ProductType = productCreateDTO.ProductType,
            Fabric = productCreateDTO.Fabric,
            OperationType = productCreateDTO.OperationType,
            OperationSide = productCreateDTO.OperationSide,
            KineticsCellular = productCreateDTO.KineticsCellular,
            KineticsRoller = productCreateDTO.KineticsRoller,
        };
    }


}