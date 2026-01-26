using Lewiss.Pricing.Shared.Customer;

namespace Lewiss.Pricing.Shared.Services.Pricing;

public class PricingService
{
    private readonly IUnitOfWork _unitOfWork;
    public PricingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }   


    public async Task CreateCustomer(CustomerDTO customerDTO)
    {
        var customer = new Data.Model.Customer
        {
           FamilyName = "",
           Street = "",
           City = "",
           Suburb = "",
           Mobile = "",
           Email = "",
           CreatedAt = DateTimeOffset.UtcNow
        };

        await _unitOfWork.Customer.AddAsync(customer);

        var worksheet = new Data.Model.Worksheet
        {
            Customer = customer,
            CustomerId = customer.Id,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _unitOfWork.Worksheet.AddAsync(worksheet);

        await _unitOfWork.CommitAsync();
    }

}