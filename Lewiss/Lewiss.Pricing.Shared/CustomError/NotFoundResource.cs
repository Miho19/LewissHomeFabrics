using FluentResults;

namespace Lewiss.Pricing.Shared.CustomError;

public class NotFoundResource : Error
{
    public NotFoundResource(string resourceName, Guid identifer) : base($"{resourceName} not found by identifer {identifer}")
    { }

    public NotFoundResource(string resourceName, string identifer) : base($"{resourceName} not found by identifer {identifer}")
    { }
}