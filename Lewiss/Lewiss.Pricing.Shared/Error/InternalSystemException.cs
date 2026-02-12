using Microsoft.AspNetCore.Http;

namespace Lewiss.Pricing.Shared.Error;

public class InternalSystemException : BaseException
{
    public InternalSystemException(string message) : base(StatusCodes.Status500InternalServerError, message)
    {
    }
}