using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.Product;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.Worksheet;

namespace Lewiss.Pricing.Shared.Services.Pricing;

public class PricingService
{
    private readonly IUnitOfWork _unitOfWork;


    public PricingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

    public virtual async Task<List<WorksheetDTO>> GetCustomerWorksheetDTOListAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        var worksheet = await _unitOfWork.Worksheet.GetWorksheetByExternalIdAsync(externalCustomerId, cancellationToken);
        if (worksheet is null)
        {
            return [];
        }

        var customer = await _unitOfWork.Customer.GetByIdAsync(worksheet.CustomerId);
        if (customer is null)
        {
            return [];
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

        return [worksheetDTO];
    }

    public virtual async Task<ProductEntryDTO?> CreateProductAsync(Guid externalWorksheetId, ProductCreateDTO productCreateDTO, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}