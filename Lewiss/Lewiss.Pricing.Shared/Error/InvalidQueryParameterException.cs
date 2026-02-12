using Lewiss.Pricing.Shared.Error;
using Microsoft.AspNetCore.Http;

namespace Lewiss.Pricing.Shared.Error;

public class InvalidQueryParameterException : BaseException
{
    public InvalidQueryParameterException(string message) : base(StatusCodes.Status400BadRequest, message) { }
}