using Lewiss.Pricing.Shared.CustomerDTO;
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
            ExternalId = Guid.CreateVersion7(DateTimeOffset.UtcNow),
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
            Id = customer.ExternalId,
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
        if(customer is null)
        {
            return null;
        }

        var worksheet = new Data.Model.Worksheet
        {
            ExternalId = Guid.CreateVersion7(DateTimeOffset.UtcNow),
            CreatedAt = DateTimeOffset.UtcNow,
            Customer = customer,
            CustomerId = customer.Id

        };

        await _unitOfWork.Worksheet.AddAsync(worksheet);
        await _unitOfWork.CommitAsync();

        var worksheetDTO = new WorksheetDTO
        {
            Id = worksheet.ExternalId,
            CustomerId = customer.ExternalId,
            Price = 0.00m,
            Additional = 0.00m
        };

        return worksheetDTO;
    }
}