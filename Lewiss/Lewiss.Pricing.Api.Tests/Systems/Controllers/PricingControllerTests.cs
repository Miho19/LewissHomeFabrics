using Lewiss.Pricing.Api.Controllers;
using Lewiss.Pricing.Api.Tests.Fixtures;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.Product;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.Services.Pricing;
using Lewiss.Pricing.Shared.Worksheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        pricingServiceMock.Setup(p => p.CreateWorksheetAsync(It.IsAny<CustomerEntryDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(new WorksheetDTO
        {
            Id = Guid.CreateVersion7(DateTimeOffset.UtcNow),
            CustomerId = customerDTO.Id,
            Price = 0.00m,
            Discount = 0.00m,
            NewBuild = false,
            CallOutFee = 0.00m
        });



        var result = await pricingController.CreateWorksheet(customerDTO);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);

        var worksheetDTO = Assert.IsType<WorksheetDTO>(createdAtActionResult.Value);
        Assert.Equal(customerDTO.Id, worksheetDTO.CustomerId);

    }

    [Fact]
    public async Task CreateWorksheet_ShouldReturnStatus500_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object);

        var customerDTO = CustomerFixture.TestCustomer;

        pricingServiceMock.Setup(p => p.CreateWorksheetAsync(It.IsAny<CustomerEntryDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync((WorksheetDTO)null!);

        var result = await pricingController.CreateWorksheet(customerDTO);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemsDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemsDetails.Status);
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnOkCreated_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object);
        var customerDTO = CustomerFixture.TestCustomer;

        pricingServiceMock.Setup(p => p.CreateCustomerAsync(It.IsAny<CustomerCreateDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(customerDTO);

        var result = await pricingController.CreateCustomer(CustomerFixture.TestCustomerCreate);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);

        var customerEntryDTO = Assert.IsType<CustomerEntryDTO>(createdAtActionResult.Value);
        Assert.Equal(customerDTO.FamilyName, customerEntryDTO.FamilyName);

    }

    [Fact]
    public async Task CreateCustomer_ShouldReturn500_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object);

        var customerCreateDTO = CustomerFixture.TestCustomerCreate;

        pricingServiceMock.Setup(p => p.CreateCustomerAsync(It.IsAny<CustomerCreateDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync((CustomerEntryDTO)null!);

        var result = await pricingController.CreateCustomer(customerCreateDTO);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemsDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemsDetails.Status);
    }

    [Fact]
    public async Task GetCustomer_ShouldReturnOK200_OnSuccess_WhenSuppliedFamilyName()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);

        pricingServiceMock.Setup(p => p.GetCustomersAsync(It.IsAny<GetCustomerQueryParameters>(), It.IsAny<CancellationToken>())).ReturnsAsync([CustomerFixture.TestCustomer]);

        var pricingController = new PricingController(pricingServiceMock.Object);
        var customer = CustomerFixture.TestCustomer;

        var queryParameters = new GetCustomerQueryParameters
        {
            FamilyName = "April"
        };

        var result = await pricingController.GetCustomer(queryParameters);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomerList = Assert.IsType<List<CustomerEntryDTO>>(okObjectResult.Value);

        var returnedCustomer = returnedCustomerList[0];

        Assert.Equal(customer.FamilyName, returnedCustomer.FamilyName);
        Assert.Equal(customer.Id, returnedCustomer.Id);
    }

    [Fact]
    public async Task GetWorksheet_ShouldReturnOK200_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var testWorksheetDTO = WorksheetFixture.TestWorksheet;

        pricingServiceMock.Setup(p => p.GetWorksheetDTOAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(testWorksheetDTO);

        var pricingController = new PricingController(pricingServiceMock.Object);

        var result = await pricingController.GetWorksheet(testWorksheetDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var worksheetDTO = Assert.IsType<WorksheetDTO>(okObjectResult.Value);
        Assert.Equal(testWorksheetDTO.Id, worksheetDTO.Id);
    }

    [Fact]
    public async Task GetWorksheet_ShouldReturn404_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var testWorksheetDTO = WorksheetFixture.TestWorksheet;

        pricingServiceMock.Setup(p => p.GetWorksheetDTOAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((WorksheetDTO)null!);

        var pricingController = new PricingController(pricingServiceMock.Object);

        var result = await pricingController.GetWorksheet(testWorksheetDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemsDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status404NotFound, problemsDetails.Status);
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturnOK200_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var testWorksheetDTO = WorksheetFixture.TestWorksheet;
        var customerDTO = CustomerFixture.TestCustomer;

        pricingServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync([testWorksheetDTO]);

        var pricingController = new PricingController(pricingServiceMock.Object);

        var result = await pricingController.GetCustomerWorksheet(customerDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var worksheetDTOList = Assert.IsType<List<WorksheetDTO>>(okObjectResult.Value);
        Assert.NotEmpty(worksheetDTOList);
        Assert.Equal(testWorksheetDTO.Id, worksheetDTOList[0].Id);
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturn500_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var testWorksheetDTO = WorksheetFixture.TestWorksheet;
        var customerDTO = CustomerFixture.TestCustomer;

        pricingServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((List<WorksheetDTO>)null!);

        var pricingController = new PricingController(pricingServiceMock.Object);

        var result = await pricingController.GetCustomerWorksheet(customerDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemsDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemsDetails.Status);
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturn200Ok_OnSuccess_WhenEmptyListIsReturned()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var testWorksheetDTO = WorksheetFixture.TestWorksheet;
        var customerDTO = CustomerFixture.TestCustomer;

        pricingServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var pricingController = new PricingController(pricingServiceMock.Object);

        var result = await pricingController.GetCustomerWorksheet(customerDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        var workoutDTOList = Assert.IsType<List<WorksheetDTO>>(okObjectResult.Value);
        Assert.Empty(workoutDTOList);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnOK200_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);


        var testWorksheetDTO = WorksheetFixture.TestWorksheet;

        var newProductDTO = ProductFixture.TestProductCreateDTOKineticsCellular;

        var pricingController = new PricingController(pricingServiceMock.Object);

        var result = await pricingController.CreateProduct(testWorksheetDTO.Id, newProductDTO);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var worksheetDTOList = Assert.IsType<List<WorksheetDTO>>(okObjectResult.Value);
        Assert.NotEmpty(worksheetDTOList);
        Assert.Equal(testWorksheetDTO.Id, worksheetDTOList[0].Id);
    }

}