using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.WorksheetDTO;
using Microsoft.Extensions.Logging;

namespace Lewiss.Pricing.Shared.Services;

public class CustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CustomerService> _logger;
    public CustomerService(IUnitOfWork unitOfWork, ILogger<CustomerService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public virtual async Task<CustomerEntryOutputDTO?> CreateCustomerAsync(CustomerCreateInputDTO customerCreateDTO, CancellationToken cancellationToken = default)
    {

        // Query database for duplicate entry and just return the entry if found
        var queryParameters = new GetCustomerQueryParameters
        {
            FamilyName = customerCreateDTO.FamilyName,
            Mobile = customerCreateDTO.Mobile,
            Email = customerCreateDTO.Email,
        };

        var queryCustomerList = await GetCustomersAsync(queryParameters, cancellationToken);

        if (queryCustomerList.Count != 0)
        {
            return queryCustomerList[0];
        }


        var customer = customerCreateDTO.ToCustomerEntity();
        await _unitOfWork.Customer.AddAsync(customer);
        await _unitOfWork.CommitAsync();


        var customerEntryDto = customer.ToEntryDTO();
        return customerEntryDto;

    }


    public virtual async Task<List<CustomerEntryOutputDTO>> GetCustomersAsync(GetCustomerQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (familyName, mobile, email) = queryParameters;
        var filteredCustomerList = await _unitOfWork.Customer.GetCustomerByQueryableParameters(familyName, mobile, email, cancellationToken);

        if (filteredCustomerList.Count == 0)
        {
            return [];
        }

        var filteredCustomerEntryDTOList = filteredCustomerList.Select(c => c.ToEntryDTO()).ToList();

        return filteredCustomerEntryDTOList;


    }

    public virtual async Task<List<WorksheetOutputDTO>> GetCustomerWorksheetDTOListAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        var worksheetList = await _unitOfWork.Worksheet.GetWorksheetsByExternalCustomerIdAsync(externalCustomerId, cancellationToken);
        if (worksheetList is null)
        {
            return null;
        }

        if (worksheetList.Count == 0)
        {
            return [];
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