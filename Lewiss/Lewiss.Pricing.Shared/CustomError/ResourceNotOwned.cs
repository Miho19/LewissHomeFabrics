using FluentResults;
namespace Lewiss.Pricing.Shared.CustomError;

public class ResourceNotOwned : Error
{
    public ResourceNotOwned(string parentResource, Guid parentIdentifer, string childResource, Guid childIdentifier) : base($"{parentResource} with identifer {parentIdentifer} does not own {childResource} with identifer {childIdentifier}") { }
}