using Lewiss.Pricing.Api.Controllers;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.Services.Pricing;
using Lewiss.Pricing.Shared.Worksheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Abstractions;

namespace Lewiss.Pricing.Api.Tests.Systems.Controllers;

public class PricingControllerTests
{
    private readonly ITestOutputHelper _logger;
    public PricingControllerTests(ITestOutputHelper logger)
    {
        _logger = logger;
    }

    [Fact]
    public async Task CreateWorksheet_ShouldReturnOkCreated_OnSuccess()
    {   
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object);

        var customerDTO = CustomerFixture.TestCustomer;

        pricingServiceMock.Setup(p => p.CreateWorksheet(It.IsAny<CustomerEntryDTO>())).ReturnsAsync(new WorksheetDTO
        {
            Id = Guid.CreateVersion7(DateTimeOffset.UtcNow),
            CustomerId = customerDTO.Id,
            Price = 0.00m,
            Additional = 0.00m
        });


        
        var result = await pricingController.CreateWorksheet(customerDTO);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
        
        var worksheetDTO = Assert.IsType<WorksheetDTO>(createdAtActionResult.Value);
        Assert.Equal(customerDTO.Id, worksheetDTO.CustomerId);

    }
    
    [Fact]
    public async Task CreateCustomer_ShouldReturnOkCreated_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object);
        var customerDTO = CustomerFixture.TestCustomer;

        pricingServiceMock.Setup(p => p.CreateCustomer(It.IsAny<CustomerCreateDTO>())).ReturnsAsync(customerDTO);

        var result = await pricingController.CreateCustomer(CustomerFixture.TestCustomerCreate);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);

        var customerEntryDTO = Assert.IsType<CustomerEntryDTO>(result);
        Assert.Equal(customerDTO.FamilyName,customerEntryDTO.FamilyName);

    }

}