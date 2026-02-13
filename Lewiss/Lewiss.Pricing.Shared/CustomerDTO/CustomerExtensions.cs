using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Shared.CustomerDTO;

public static class CustomerExtensions
{
    public static CustomerEntryOutputDTO ToEntryDTO(this Customer customer)
    {
        return new CustomerEntryOutputDTO
        {
            Id = customer.ExternalMapping,
            FamilyName = customer.FamilyName,
            Street = customer.Street,
            City = customer.City,
            Suburb = customer.Suburb,
            Mobile = customer.Mobile,
            Email = customer.Email,
        };
    }

    public static Customer ToCustomerEntity(this CustomerCreateInputDTO customerCreateDTO)
    {
        return new Customer
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
    }


}