using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.Product;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.Worksheet;

namespace Lewiss.Pricing.Shared.Services;

public class PricingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductService _productService;


    public PricingService(IUnitOfWork unitOfWork, ProductService productService)
    {
        _unitOfWork = unitOfWork;
        _productService = productService;
    }


    public virtual async Task<CustomerEntryDTO?> CreateCustomerAsync(CustomerCreateDTO customerCreateDTO, CancellationToken cancellationToken = default)
    {

        var customer = new Data.Model.Customer
        {
            ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
            FamilyName = customerCreateDTO.FamilyName,
            Street = customerCreateDTO.Street,
            City = customerCreateDTO.City,
            Suburb = customerCreateDTO.Suburb,
            Mobile = customerCreateDTO.Mobile,
            Email = customerCreateDTO.Email,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _unitOfWork.Customer.AddAsync(customer);
        await _unitOfWork.CommitAsync();

        var customerEntryDto = new CustomerEntryDTO
        {
            Id = customer.ExternalMapping,
            FamilyName = customer.FamilyName,
            Street = customer.Street,
            City = customer.City,
            Suburb = customer.Suburb,
            Mobile = customer.Mobile,
            Email = customer.Email,
        };

        return customerEntryDto;
    }

    public virtual async Task<WorksheetDTO?> CreateWorksheetAsync(CustomerEntryDTO customerEntryDTO, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(customerEntryDTO.Id, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var worksheet = new Data.Model.Worksheet
        {
            ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
            CreatedAt = DateTimeOffset.UtcNow,
            Customer = customer,
            CustomerId = customer.CustomerId,
            CallOutFee = 0.00m,
            Discount = 0.00m,
            NewBuild = false,
            Price = 0.00m,
        };

        await _unitOfWork.Worksheet.AddAsync(worksheet);
        await _unitOfWork.CommitAsync();

        var worksheetDTO = new WorksheetDTO
        {
            Id = worksheet.ExternalMapping,
            CustomerId = customer.ExternalMapping,
            Price = 0.00m,
            Discount = 0.00m,
            NewBuild = false,
            CallOutFee = 0.00m
        };

        return worksheetDTO;
    }

    public virtual async Task<List<CustomerEntryDTO>?> GetCustomersAsync(GetCustomerQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (familyName, mobile, email) = queryParameters;
        var filteredCustomerList = await _unitOfWork.Customer.GetCustomerByQueryableParameters(familyName, mobile, email, cancellationToken);
        if (filteredCustomerList is null) return null;

        var filteredCustomerEntryDTOList = filteredCustomerList.Select(c => new CustomerEntryDTO
        {
            Id = c.ExternalMapping,
            FamilyName = c.FamilyName,
            Street = c.Street,
            City = c.City,
            Suburb = c.Suburb,
            Mobile = c.Mobile,
            Email = c.Email
        }).ToList();

        return filteredCustomerEntryDTOList;
    }

    public virtual async Task<WorksheetDTO?> GetWorksheetDTOAsync(Guid externalWorksheetId, CancellationToken cancellationToken = default)
    {
        var worksheet = await _unitOfWork.Worksheet.GetWorksheetByExternalIdAsync(externalWorksheetId, cancellationToken);
        if (worksheet is null)
        {
            return null;
        }


        var customer = await _unitOfWork.Customer.GetByIdAsync(worksheet.CustomerId);
        if (customer is null)
        {
            return null;
        }

        var worksheetDTO = new WorksheetDTO
        {
            Id = worksheet.ExternalMapping,
            CustomerId = customer.ExternalMapping,
            CallOutFee = worksheet.CallOutFee,
            Discount = worksheet.Discount,
            NewBuild = worksheet.NewBuild,
            Price = worksheet.Price
        };
        return worksheetDTO;
    }

    public virtual async Task<List<WorksheetDTO>?> GetCustomerWorksheetDTOListAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        var worksheetList = await _unitOfWork.Worksheet.GetWorksheetsByExternalCustomerIdAsync(externalCustomerId, cancellationToken);
        if (worksheetList is null)
        {
            return null;
        }

        var worksheetDTOList = worksheetList.Select(w => new WorksheetDTO
        {
            Id = w.ExternalMapping,
            CustomerId = externalCustomerId,
            CallOutFee = w.CallOutFee,
            Discount = w.Discount,
            NewBuild = w.NewBuild,
            Price = w.Price
        }).ToList();

        return worksheetDTOList;
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

        product = await PopulateProductOptionVariationsList(product, productCreateDTO, cancellationToken);
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
            GeneralConfiguration = productCreateDTO.GeneralProductConfigration
        };

        return productEntryDTO;
    }

    private async Task<Data.Model.Product?> PopulateProductOptionVariationsList(Data.Model.Product product, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        if (product.OptionVariations is null)
        {
            product.OptionVariations = new List<OptionVariation>();
        }


        return null;
    }
}