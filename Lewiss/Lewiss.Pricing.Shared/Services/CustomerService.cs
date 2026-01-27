using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.QueryParameters;

namespace Lewiss.Pricing.Shared.Services;

public class CustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Persists a new customer to the database asynchronously.
    /// </summary>
    /// <param name="customerCreateDTO">The customer details to persist</param>
    /// <param name="cancellationToken">
    /// A token that can be used to propagate notification that the operation should be canceled.
    /// </param>
    /// <remarks>
    /// Adds the new customer to the database if it is not already present.
    /// Changes are tracked and committed by the injected <see cref="IUnitOfWork"/>
    /// </remarks>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, returning the CustomerEntryDTO.
    /// </returns>
    /// Needs to be updated to try catch instead of using any to check for duplication
    public virtual async Task<CustomerEntryDTO?> CreateCustomerAsync(CustomerCreateDTO customerCreateDTO, CancellationToken cancellationToken = default)
    {

        Customer customer;

        var queryDuplicatedCustomerList = await _unitOfWork.Customer.GetCustomerByQueryableParameters(
            customerCreateDTO.FamilyName,
            customerCreateDTO.Mobile,
            customerCreateDTO.Email,
            cancellationToken
        );

        if (queryDuplicatedCustomerList is not null && queryDuplicatedCustomerList.Count > 0)
        {
            customer = queryDuplicatedCustomerList[0];
        }
        else
        {
            customer = new Customer
            {
                ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
                FamilyName = customerCreateDTO.FamilyName,
                Street = customerCreateDTO.Street,
                City = customerCreateDTO.City,
                Suburb = customerCreateDTO.Suburb,
                Mobile = customerCreateDTO.Mobile,
                Email = customerCreateDTO.Email,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await _unitOfWork.Customer.AddAsync(customer);
            await _unitOfWork.CommitAsync();
        }

        var customerEntryDto = new CustomerEntryDTO
        {
            Id = customer.ExternalMapping,
            FamilyName = customer.FamilyName,
            Street = customer.Street,
            City = customer.City,
            Suburb = customer.Suburb,
            Mobile = customer.Mobile,
            Email = customer.Email,
        };

        return customerEntryDto;

    }


    public virtual async Task<List<CustomerEntryDTO>?> GetCustomersAsync(GetCustomerQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (familyName, mobile, email) = queryParameters;
        var filteredCustomerList = await _unitOfWork.Customer.GetCustomerByQueryableParameters(familyName, mobile, email, cancellationToken);
        if (filteredCustomerList is null) return null;

        var filteredCustomerEntryDTOList = filteredCustomerList.Select(c => new CustomerEntryDTO
        {
            Id = c.ExternalMapping,
            FamilyName = c.FamilyName,
            Street = c.Street,
            City = c.City,
            Suburb = c.Suburb,
            Mobile = c.Mobile,
            Email = c.Email
        }).ToList();

        return filteredCustomerEntryDTOList;
    }


}