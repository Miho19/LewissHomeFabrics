using Lewiss.Pricing.Shared.CustomerDTO;

namespace Lewiss.Pricing.Shared.Services.Pricing;

public class PricingService
{
    private readonly IUnitOfWork _unitOfWork;
    public PricingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }   


    public async Task<CustomerEntryDTO> CreateCustomer(CustomerCreateDTO customerCreateDTO)
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

}