using FluentResults;
using Lewiss.Pricing.Shared.QueryParameters;

namespace Lewiss.Pricing.Shared.CustomError;

public class CustomerAlreadyExists : Error
{
    public CustomerAlreadyExists(string resourceName, GetCustomerQueryParameters identifer) : base($"{resourceName} with identifer {identifer.ToString()} already exists.")
    {

    }
}