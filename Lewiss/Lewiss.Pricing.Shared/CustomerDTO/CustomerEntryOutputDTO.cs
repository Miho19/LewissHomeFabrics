namespace Lewiss.Pricing.Shared.CustomerDTO;

public record CustomerEntryOutputDTO
{
    public required Guid Id { get; set; }
    public required string FamilyName { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string Suburb { get; set; }
    public required string Mobile { get; set; }
    public required string Email { get; set; }

}