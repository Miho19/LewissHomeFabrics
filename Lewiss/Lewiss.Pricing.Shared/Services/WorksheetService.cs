using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.WorksheetDTO;

namespace Lewiss.Pricing.Shared.Services;

public class WorksheetService
{
    private readonly IUnitOfWork _unitOfWork;
    public WorksheetService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public virtual async Task<WorksheetOutputDTO?> CreateWorksheetAsync(Guid externalCustomerId, CancellationToken cancellationToken = default)
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

        var worksheetDTO = worksheet.ToWorksheetDTO(externalCustomerId);

        return worksheetDTO;
    }

    private async Task<(Customer?, Worksheet?)> GetCustomerAndWorksheetAsync(Guid externalCustomerId, Guid externalWorksheetId, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customer.GetCustomerByExternalIdAsync(externalCustomerId, cancellationToken);
        if (customer is null)
        {
            return (null, null);
        }

        var worksheet = await _unitOfWork.Worksheet.GetWorksheetByExternalIdAsync(externalWorksheetId, cancellationToken);
        if (worksheet is null)
        {
            return (null, null);
        }

        if (worksheet.CustomerId != customer.CustomerId)
        {
            return (null, null);
        }

        return (customer, worksheet);
    }


    public virtual async Task<WorksheetOutputDTO?> GetWorksheetAsync(Guid externalCustomerId, Guid externalWorksheetId, CancellationToken cancellationToken = default)
    {

        var (customer, worksheet) = await GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId);
        if (customer is null || worksheet is null)
        {
            return null;
        }

        var worksheetDTO = worksheet.ToWorksheetDTO(externalCustomerId);
        return worksheetDTO;
    }

    public virtual async Task<List<ProductEntryOutputDTO>?> GetWorksheetProductsAsync(Guid externalCustomerId, Guid externalWorksheetId, CancellationToken cancellationToken = default)
    {
        var (customer, worksheet) = await GetCustomerAndWorksheetAsync(externalCustomerId, externalWorksheetId);
        if (customer is null || worksheet is null)
        {
            return null;
        }

        var productList = await _unitOfWork.Worksheet.GetWorksheetProductsAsync(worksheet, cancellationToken);

        var productEntryDTOList = productList.Select(p => p.ToProductEntryDTO(worksheet.ExternalMapping)).ToList();

        return productEntryDTOList;
    }

}