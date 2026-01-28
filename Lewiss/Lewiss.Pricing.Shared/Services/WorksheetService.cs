using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.Worksheet;

namespace Lewiss.Pricing.Shared.Services;

public class WorksheetService
{
    private readonly IUnitOfWork _unitOfWork;
    public WorksheetService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public virtual async Task<WorksheetDTO?> CreateWorksheetAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(externalCustomerId, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var worksheet = new Data.Model.Worksheet
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

        var worksheetDTO = new WorksheetDTO
        {
            Id = worksheet.ExternalMapping,
            CustomerId = customer.ExternalMapping,
            Price = 0.00m,
            Discount = 0.00m,
            NewBuild = false,
            CallOutFee = 0.00m
        };

        return worksheetDTO;
    }

    public virtual async Task<WorksheetDTO?> GetWorksheetAsync(Guid externalCustomerId, Guid externalWorksheetId, CancellationToken cancellationToken = default)
    {

        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(externalCustomerId, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var worksheet = await _unitOfWork.Worksheet.GetWorksheetByExternalIdAsync(externalWorksheetId, cancellationToken);
        if (worksheet is null)
        {
            return null;
        }

        if (worksheet.CustomerId != customer.CustomerId)
        {
            return null;
        }

        var worksheetDTO = worksheet.ToWorksheetDTO(externalCustomerId);
        return worksheetDTO;
    }

}