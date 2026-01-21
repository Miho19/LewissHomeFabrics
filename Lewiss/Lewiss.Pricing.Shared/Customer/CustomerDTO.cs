namespace Lewiss.Pricing.Shared.Customer;
public record CustomerDTO
{
    public required string FamilyName {get; set;}
    public required string Street {get; set;}
    public required string Suburb {get; set;}
    public required string Mobile {get; set;}
    public required string Email {get; set;}
    public required string Consultant {get; set;}
    public required string Measurer {get; set;}
   
}