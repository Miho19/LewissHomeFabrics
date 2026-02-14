using FluentResults;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.WorksheetDTO;
using Microsoft.Extensions.Logging;

namespace Lewiss.Pricing.Shared.Services;

public class WorksheetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SharedUtilityService _sharedUtilityService;
    private readonly ILogger<WorksheetService> _logger;

    public WorksheetService(IUnitOfWork unitOfWork, SharedUtilityService sharedUtilityService, ILogger<WorksheetService> logger)
    {
        _unitOfWork = unitOfWork;
        _sharedUtilityService = sharedUtilityService;
        _logger = logger;
    }

    public virtual async Task<Result<WorksheetOutputDTO>> CreateWorksheetAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {

        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(externalCustomerId, cancellationToken);
        if (customer is null)
        {
            return Result.Fail(new NotFoundResource("Customer", externalCustomerId));
        }

        var worksheet = new Worksheet
        {
            ExternalMapping = Guid.CreateVersion7(DateTimeOffset.UtcNow),
            CreatedAt = DateTimeOffset.UtcNow,
            Customer = customer,
            CustomerId = customer.CustomerId,
            CallOutFee = 0.00m,
            Discount = 0.00m,
            NewBuild = false,
            Price = 0.00m,
        };

        await _unitOfWork.Worksheet.AddAsync(worksheet);
        await _unitOfWork.CommitAsync();

        var worksheetDTO = worksheet.ToWorksheetDTO(externalCustomerId);

        return Result.Ok(worksheetDTO);

    }


    public virtual async Task<Result<WorksheetOutputDTO>> GetWorksheetAsync(Guid externalCustomerId, Guid externalWorksheetId, CancellationToken cancellationToken = default)
    {

        var result = await _sharedUtilityService.GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId);
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        var (_, worksheet) = result.Value;
        var worksheetDTO = worksheet.ToWorksheetDTO(externalCustomerId);
        return Result.Ok(worksheetDTO);

    }

    public virtual async Task<Result<List<ProductEntryOutputDTO>>> GetWorksheetProductsAsync(Guid externalCustomerId, Guid externalWorksheetId, CancellationToken cancellationToken = default)
    {

        var result = await _sharedUtilityService.GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId);
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        var (_, worksheet) = result.Value;

        var productList = await _unitOfWork.Worksheet.GetWorksheetProductsAsync(worksheet, cancellationToken);
        if (productList.Count == 0)
        {
            return Result.Ok(new List<ProductEntryOutputDTO>());
        }

        var productEntryDTOList = productList.Select(p => p.ToProductEntryDTO(worksheet.ExternalMapping)).ToList();

        return Result.Ok(productEntryDTOList);

    }

}