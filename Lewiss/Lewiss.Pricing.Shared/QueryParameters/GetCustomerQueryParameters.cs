namespace Lewiss.Pricing.Shared.QueryParameters;

public class GetCustomerQueryParameters
{
    public string? FamilyName { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }

    public void Deconstruct(out string? familyName, out string? mobile, out string? email)
    {
        familyName = FamilyName;
        mobile = Mobile;
        email = Email;
    }

    public override string ToString()
    {
        return $"familyName: {FamilyName} mobile: {Mobile} email: {Email}";
    }
}