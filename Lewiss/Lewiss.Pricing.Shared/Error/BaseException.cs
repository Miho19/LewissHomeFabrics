
namespace Lewiss.Pricing.Shared.Error;

public class BaseException : Exception
{
    public int StatusCode { get; init; }

    public BaseException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}