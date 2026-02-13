using FluentResults;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.CustomError;
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

    public virtual async Task<Result<CustomerEntryOutputDTO>> CreateCustomerAsync(CustomerCreateInputDTO customerCreateDTO, CancellationToken cancellationToken = default)
    {

        var queryParameters = new GetCustomerQueryParameters
        {
            FamilyName = customerCreateDTO.FamilyName,
            Mobile = customerCreateDTO.Mobile,
            Email = customerCreateDTO.Email,
        };

        var customerQueryResult = await GetCustomersAsync(queryParameters, cancellationToken);

        if (customerQueryResult.IsFailed)
        {
            return Result.Fail(new Error("Exceptional error occurred."));
        }

        var queryCustomerList = customerQueryResult.Value;


        if (queryCustomerList.Count != 0)
        {
            Result.Fail(new CustomerAlreadyExists("Customer", queryParameters));
        }

        var customer = customerCreateDTO.ToCustomerEntity();
        await _unitOfWork.Customer.AddAsync(customer);
        await _unitOfWork.CommitAsync();

        var customerEntryDto = customer.ToEntryDTO();
        return Result.Ok(customerEntryDto);

    }


    public virtual async Task<Result<List<CustomerEntryOutputDTO>>> GetCustomersAsync(GetCustomerQueryParameters queryParameters, CancellationToken cancellationToken = default)
    {
        var (familyName, mobile, email) = queryParameters;
        var filteredCustomerList = await _unitOfWork.Customer.GetCustomerByQueryableParameters(familyName, mobile, email, cancellationToken);

        if (filteredCustomerList.Count == 0)
        {
            return Result.Ok(new List<CustomerEntryOutputDTO>());
        }

        var filteredCustomerEntryDTOList = filteredCustomerList.Select(c => c.ToEntryDTO()).ToList();

        return filteredCustomerEntryDTOList;


    }

    /// <summary>
    /// Currently we are relying on _unitOfWork.Worksheet.GetWorksheetsByExternalCustomerIdAsync to return null if the customer is not found... fix later
    /// </summary>
    /// <param name="externalCustomerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public virtual async Task<Result<List<WorksheetOutputDTO>>> GetCustomerWorksheetDTOListAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        var worksheetList = await _unitOfWork.Worksheet.GetWorksheetsByExternalCustomerIdAsync(externalCustomerId, cancellationToken);
        if (worksheetList is null)
        {
            return Result.Fail(new NotFoundResource("Customer", externalCustomerId));
        }

        if (worksheetList.Count == 0)
        {
            return Result.Ok(new List<WorksheetOutputDTO>());
        }

        var worksheetDTOList = worksheetList.Select(w => w.ToWorksheetDTO(externalCustomerId)).ToList();

        return worksheetDTOList;


    }

    public virtual async Task<Result<CustomerEntryOutputDTO>> GetCustomerByExternalIdAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {

        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(externalCustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Fail(new NotFoundResource("Customer", externalCustomerId));
        }

        var customerEntryDto = customer.ToEntryDTO();

        return customerEntryDto;
    }

}