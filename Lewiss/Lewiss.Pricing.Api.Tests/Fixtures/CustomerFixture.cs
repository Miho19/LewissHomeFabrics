using Lewiss.Pricing.Shared.CustomerDTO;

namespace Lewiss.Pricing.Api.Tests.Fixtures;
public static class CustomerFixture
{
    
    public static CustomerEntryDTO TestCustomer {get;} = new CustomerEntryDTO
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
    public static CustomerCreateDTO TestCustomerCreate {get;} = new CustomerCreateDTO
    {
      FamilyName = "April",
        Street = "Street Address",
       City = "City",
       Suburb = "Suburb",
       Mobile = "123 458 7891",
       Email = "email.address@domain",
    };

}