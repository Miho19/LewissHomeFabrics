

using FluentResults;


/**
    This will be replaced by a validation error library soon
*/
namespace Lewiss.Pricing.Shared.CustomError;

public class ValidationError : Error
{
    public ValidationError(string field, object value) : base($"{field} does not accept {value}") { }
}