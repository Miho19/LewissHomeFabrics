
using Microsoft.AspNetCore.Http;

namespace Lewiss.Pricing.Shared.Error;

public class NotFoundException : BaseException
{
    public NotFoundException(string message) : base(StatusCodes.Status404NotFound, message)
    {
    }
}