using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.WorksheetDTO;

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
    public virtual async Task<CustomerEntryOutputDTO?> CreateCustomerAsync(CustomerCreateInputDTO customerCreateDTO, CancellationToken cancellationToken = default)
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
            customer = customerCreateDTO.ToCustomerEntity();
            await _unitOfWork.Customer.AddAsync(customer);
            await _unitOfWork.CommitAsync();
        }

        var customerEntryDto = customer.ToEntryDTO();
        return customerEntryDto;

    }


    public virtual async Task<List<CustomerEntryOutputDTO>?> GetCustomersAsync(GetCustomerQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (familyName, mobile, email) = queryParameters;
        var filteredCustomerList = await _unitOfWork.Customer.GetCustomerByQueryableParameters(familyName, mobile, email, cancellationToken);
        if (filteredCustomerList is null) return null;

        var filteredCustomerEntryDTOList = filteredCustomerList.Select(c => c.ToEntryDTO()).ToList();

        return filteredCustomerEntryDTOList;
    }

    public virtual async Task<List<WorksheetOutputDTO>?> GetCustomerWorksheetDTOListAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        var worksheetList = await _unitOfWork.Worksheet.GetWorksheetsByExternalCustomerIdAsync(externalCustomerId, cancellationToken);
        if (worksheetList is null)
        {
            return null;
        }

        var worksheetDTOList = worksheetList.Select(w => w.ToWorksheetDTO(externalCustomerId)).ToList();

        return worksheetDTOList;
    }

    public virtual async Task<CustomerEntryOutputDTO?> GetCustomerByExternalIdAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(externalCustomerId, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var customerEntryDto = customer.ToEntryDTO();

        return customerEntryDto;
    }

}