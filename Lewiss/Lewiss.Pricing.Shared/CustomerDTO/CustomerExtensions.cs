using Lewiss.Pricing.Data.Model;

namespace Lewiss.Pricing.Shared.CustomerDTO;

public static class CustomerExtensions
{
    public static CustomerEntryDTO ToEntryDTO(this Customer customer)
    {
        if (customer is null)
        {
            throw new Exception("Customer object is null");
        }

        return new CustomerEntryDTO
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

    public static Customer ToCustomerEntity(this CustomerCreateDTO customerCreateDTO)
    {
        if (customerCreateDTO is null)
        {
            throw new Exception("Customer Create DTO is null");
        }

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