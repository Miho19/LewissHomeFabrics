using Lewiss.Pricing.Shared.Customer;

public static class CustomerFixture
{
   public static CustomerDTO TestCustomer {get;} = new CustomerDTO()
   {
       FamilyName = "April",
       Street = "Street Address",
       Suburb = "Suburb",
       Mobile = "123 458 7891",
       Email = "email.address@domain",
       Consultant = "Consultant",
       Measurer = "Measurer",
       CreatedAt = new DateTime(2026, 1, 21, 10, 20, 20)
   };

   

}