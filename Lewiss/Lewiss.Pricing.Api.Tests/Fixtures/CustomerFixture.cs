using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.CustomerDTO;

namespace Lewiss.Pricing.Api.Tests.Fixtures;

public static class CustomerFixture
{

    public static CustomerEntryDTO TestCustomerEntryDTO { get; } = new CustomerEntryDTO
    {
        FamilyName = "April",
        Street = "Street Address",
        City = "City",
        Suburb = "Suburb",
        Mobile = "123 458 7891",
        Email = "email.address@domain",
        Id = Guid.CreateVersion7(DateTimeOffset.UtcNow)
    };

    // prettier not working for some reason
    public static CustomerCreateDTO TestCustomerCreate { get; } = new CustomerCreateDTO
    {
        FamilyName = TestCustomerEntryDTO.FamilyName,
        Street = TestCustomerEntryDTO.Street,
        City = TestCustomerEntryDTO.City,
        Suburb = TestCustomerEntryDTO.Suburb,
        Mobile = TestCustomerEntryDTO.Mobile,
        Email = TestCustomerEntryDTO.Email,
    };

    public static Customer TestCustomer = new Customer
    {
        CustomerId = 1,
        ExternalMapping = TestCustomerEntryDTO.Id,
        FamilyName = TestCustomerEntryDTO.FamilyName,
        Street = TestCustomerEntryDTO.Street,
        City = TestCustomerEntryDTO.City,
        Suburb = TestCustomerEntryDTO.Suburb,
        Mobile = TestCustomerEntryDTO.Mobile,
        Email = TestCustomerEntryDTO.Email,
        CreatedAt = DateTimeOffset.UtcNow,
    };

}