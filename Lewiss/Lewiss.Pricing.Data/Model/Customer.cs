namespace Lewiss.Pricing.Data.Model;

public class Customer
{
    public int CustomerId { get; set; }
    public required Guid ExternalMapping { get; set; }
    public required string FamilyName { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string Suburb { get; set; }
    public required string Mobile { get; set; }
    public required string Email { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }

    public ICollection<Worksheet> CurrentWorksheets { get; set; } = new List<Worksheet>();


}