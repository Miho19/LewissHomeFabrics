using Lewiss.Pricing.Shared.Customer;

public static class CustomerFixture
{
    private static readonly TimeSpan NZDTimeSpanOffset = new TimeSpan(13, 0, 0);
    public static CustomerDTO TestCustomer {get;} = new CustomerDTO()
    {
       FamilyName = "April",
       Street = "Street Address",
       Suburb = "Suburb",
       Mobile = "123 458 7891",
       Email = "email.address@domain",
       Consultant = "Consultant",
       Measurer = "Measurer",
       CreatedAt = new DateTimeOffset(2026, 1, 21, 10, 20, 20, NZDTimeSpanOffset)
    };

}