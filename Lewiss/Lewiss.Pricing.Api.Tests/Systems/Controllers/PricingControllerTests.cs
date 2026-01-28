using Lewiss.Pricing.Api.Controllers;
using Lewiss.Pricing.Api.Tests.Fixtures;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.Services;
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
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        worksheetServiceMock.Setup(p => p.CreateWorksheetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(WorksheetFixture.TestWorksheetDTO);

        var result = await pricingController.CreateWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);

        var worksheetDTO = Assert.IsType<WorksheetDTO>(createdAtActionResult.Value);
        Assert.Equal(testCustomerEntryDTO.Id, worksheetDTO.CustomerId);

    }

    [Fact]
    public async Task CreateWorksheet_ShouldReturnStatus500_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        worksheetServiceMock.Setup(p => p.CreateWorksheetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((WorksheetDTO)null!);

        var result = await pricingController.CreateWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemsDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemsDetails.Status);
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnOkCreated_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.CreateCustomerAsync(It.IsAny<CustomerCreateDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(testCustomerEntryDTO);
        var result = await pricingController.CreateCustomer(CustomerFixture.TestCustomerCreate);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
        var customerEntryDTO = Assert.IsType<CustomerEntryDTO>(createdAtActionResult.Value);
        Assert.Equal(testCustomerEntryDTO.FamilyName, customerEntryDTO.FamilyName);
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturn500_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testCustomerEntryDTO = CustomerFixture.TestCustomerCreate;

        customerServiceMock.Setup(p => p.CreateCustomerAsync(It.IsAny<CustomerCreateDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync((CustomerEntryDTO)null!);

        var result = await pricingController.CreateCustomer(testCustomerEntryDTO);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemsDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemsDetails.Status);
    }

    [Fact]
    public async Task GetCustomer_ShouldReturnOK200_OnSuccess_WhenSuppliedValidCustomerId()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.GetCustomerByExternalIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(testCustomerEntryDTO);

        var result = await pricingController.GetCustomer(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomerEntryDTO = Assert.IsType<CustomerEntryDTO>(okObjectResult.Value);
        Assert.Equal(testCustomerEntryDTO.FamilyName, returnedCustomerEntryDTO.FamilyName);
        Assert.Equal(testCustomerEntryDTO.Id, returnedCustomerEntryDTO.Id);
    }

    [Fact]
    public async Task GetCustomer_ShouldReturnNotFound404_OnFailure_WhenSuppliedInvalidCustomerId()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.GetCustomerByExternalIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((CustomerEntryDTO)null!);

        var result = await pricingController.GetCustomer(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemsDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status404NotFound, problemsDetails.Status);
    }

    [Fact]
    public async Task GetWorksheet_ShouldReturnOK200_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        worksheetServiceMock.Setup(p => p.GetWorksheetAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(testWorksheetDTO);

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        var result = await pricingController.GetWorksheet(testCustomerEntryDTO.Id, testWorksheetDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var worksheetDTO = Assert.IsType<WorksheetDTO>(okObjectResult.Value);
        Assert.Equal(testWorksheetDTO.Id, worksheetDTO.Id);
    }

    [Fact]
    public async Task GetWorksheet_ShouldReturn404_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;

        worksheetServiceMock.Setup(p => p.GetWorksheetAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((WorksheetDTO)null!);

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;
        var result = await pricingController.GetWorksheet(testCustomerEntryDTO.Id, testWorksheetDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemsDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status404NotFound, problemsDetails.Status);
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturnOK200_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync([testWorksheetDTO]);

        var result = await pricingController.GetCustomerWorksheet(testCustomerEntryDTO.Id);

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
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((List<WorksheetDTO>)null!);


        var result = await pricingController.GetCustomerWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        var problemsDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status500InternalServerError, problemsDetails.Status);
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturn200Ok_OnSuccess_WhenEmptyListIsReturned()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
        var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object);
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object);
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object);
        var pricingController = new PricingController(pricingServiceMock.Object, customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var result = await pricingController.GetCustomerWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        var workoutDTOList = Assert.IsType<List<WorksheetDTO>>(okObjectResult.Value);
        Assert.Empty(workoutDTOList);
    }


    // public async Task CreateProduct_ShouldReturnOK200_OnSuccess()
    // {
    //     var unitOfWorkMock = new Mock<IUnitOfWork>();
    //     var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object);
    //     var pricingServiceMock = new Mock<PricingService>(unitOfWorkMock.Object, productServiceMock.Object);

    //     var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;

    //     var newProductDTO = ProductFixture.TestProductCreateDTOKineticsCellular;

    //     var pricingController = new PricingController(pricingServiceMock.Object);

    //     var result = await pricingController.CreateProduct(testWorksheetDTO.Id, newProductDTO);

    //     Assert.NotNull(result);
    //     var okObjectResult = Assert.IsType<OkObjectResult>(result);
    //     var worksheetDTOList = Assert.IsType<List<WorksheetDTO>>(okObjectResult.Value);
    //     Assert.NotEmpty(worksheetDTOList);
    //     Assert.Equal(testWorksheetDTO.Id, worksheetDTOList[0].Id);
    // }

}