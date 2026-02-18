using FluentResults;
using Lewiss.Pricing.Api.Controllers;
using Lewiss.Pricing.Api.Tests.Fixtures;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.ProductStrategy;
using Lewiss.Pricing.Shared.Services;
using Lewiss.Pricing.Shared.WorksheetDTO;
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

    private class PricingControllerMocks
    {
        public Mock<IUnitOfWork> UnitOfWorkMock;

        public Mock<ILogger<CustomerService>> CustomerServiceLoggerMock;

        public Mock<CustomerService> CustomerServiceMock;

        public Mock<SharedUtilityService> SharedUtilityServiceMock;

        public Mock<IServiceProvider> ServiceProviderMock;

        public Mock<ProductStrategyResolver> ProductStrategyResolverMock;

        public Mock<ILogger<FabricService>> FabricServiceLoggerMock;
        public Mock<FabricService> FabricServiceMock;

        public Mock<ILogger<ProductService>> ProductServiceLoggerMock;

        public Mock<ProductService> ProductServiceMock;

        public Mock<ILogger<WorksheetService>> WorksheetServiceLoggerMock;
        public Mock<WorksheetService> WorksheetServiceMock;

        public PricingController PricingController;

        public PricingControllerMocks()
        {
            UnitOfWorkMock = new();
            CustomerServiceLoggerMock = new();
            CustomerServiceMock = new(UnitOfWorkMock.Object, CustomerServiceLoggerMock.Object);
            SharedUtilityServiceMock = new(UnitOfWorkMock.Object);
            ServiceProviderMock = new();
            ProductStrategyResolverMock = new(ServiceProviderMock.Object);
            FabricServiceLoggerMock = new();
            FabricServiceMock = new(UnitOfWorkMock.Object, SharedUtilityServiceMock.Object, ProductStrategyResolverMock.Object, FabricServiceLoggerMock.Object);
            ProductServiceLoggerMock = new();
            ProductServiceMock = new(UnitOfWorkMock.Object, SharedUtilityServiceMock.Object, ProductStrategyResolverMock.Object, ProductServiceLoggerMock.Object);
            WorksheetServiceLoggerMock = new();
            WorksheetServiceMock = new(UnitOfWorkMock.Object, SharedUtilityServiceMock.Object, ProductStrategyResolverMock.Object, WorksheetServiceLoggerMock.Object);
            PricingController = new(CustomerServiceMock.Object, ProductServiceMock.Object, WorksheetServiceMock.Object);
        }

    }



    [Fact]
    public async Task CreateWorksheet_ShouldReturnOkCreated_OnSuccess()
    {

        var pricingControllerMocks = new PricingControllerMocks();

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        pricingControllerMocks.WorksheetServiceMock.Setup(p => p.CreateWorksheetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(WorksheetFixture.TestWorksheetDTO));

        var result = await pricingControllerMocks.PricingController.CreateWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);

        var worksheetDTO = Assert.IsType<WorksheetOutputDTO>(createdAtActionResult.Value);
        Assert.Equal(testCustomerEntryDTO.Id, worksheetDTO.CustomerId);

    }

    [Fact]
    public async Task CreateWorksheet_ShouldReturnStatus500_OnFailure()
    {
        var pricingControllerMocks = new PricingControllerMocks();



        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        pricingControllerMocks.WorksheetServiceMock.Setup(p => p.CreateWorksheetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new NotFoundResource("Customer", testCustomerEntryDTO.Id)));

        var result = await pricingControllerMocks.PricingController.CreateWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Customer", objectResult.Value.ToString());
    }






    [Fact]
    public async Task GetWorksheet_ShouldReturnOK200_OnSuccess()
    {
        var pricingControllerMocks = new PricingControllerMocks();


        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        pricingControllerMocks.WorksheetServiceMock.Setup(p => p.GetWorksheetAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(testWorksheetDTO));

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        var result = await pricingControllerMocks.PricingController.GetWorksheet(testCustomerEntryDTO.Id, testWorksheetDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var worksheetDTO = Assert.IsType<WorksheetOutputDTO>(okObjectResult.Value);
        Assert.Equal(testWorksheetDTO.Id, worksheetDTO.Id);
    }

    [Fact]
    public async Task GetWorksheet_ShouldReturn500_OnFailure()
    {
        var pricingControllerMocks = new PricingControllerMocks();


        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;

        pricingControllerMocks.WorksheetServiceMock.Setup(p => p.GetWorksheetAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new NotFoundResource("Worksheet", testWorksheetDTO.Id)));

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;
        var result = await pricingControllerMocks.PricingController.GetWorksheet(testCustomerEntryDTO.Id, testWorksheetDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Worksheet", objectResult.Value.ToString());
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturnOK200_OnSuccess()
    {
        var pricingControllerMocks = new PricingControllerMocks();



        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        pricingControllerMocks.CustomerServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(new List<WorksheetOutputDTO>
        {
            testWorksheetDTO
        }));

        var result = await pricingControllerMocks.PricingController.GetCustomerWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var worksheetDTOList = Assert.IsType<List<WorksheetOutputDTO>>(okObjectResult.Value);
        Assert.NotEmpty(worksheetDTOList);
        Assert.Equal(testWorksheetDTO.Id, worksheetDTOList[0].Id);
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturn500_OnFailure()
    {
        var pricingControllerMocks = new PricingControllerMocks();


        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        pricingControllerMocks.CustomerServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new NotFoundResource("Customer", testCustomerEntryDTO.Id)));


        var result = await pricingControllerMocks.PricingController.GetCustomerWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Customer", objectResult.Value.ToString());
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturn200Ok_OnSuccess_WhenEmptyListIsReturned()
    {
        var pricingControllerMocks = new PricingControllerMocks();


        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        pricingControllerMocks.CustomerServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(new List<WorksheetOutputDTO>()));

        var result = await pricingControllerMocks.PricingController.GetCustomerWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        var workoutDTOList = Assert.IsType<List<WorksheetOutputDTO>>(okObjectResult.Value);
        Assert.Empty(workoutDTOList);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnOK200_OnSuccess()
    {
        var pricingControllerMocks = new PricingControllerMocks();


        pricingControllerMocks.ProductServiceMock.Setup(p => p.CreateProductAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ProductCreateInputDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(ProductFixture.TestProductEntryDTOKineticsCellular));

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var newProductDTO = ProductFixture.TestProductCreateInputDTOKineticsCellular;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        var result = await pricingControllerMocks.PricingController.CreateProduct(testCustomerEntryDTO.Id, testWorksheetDTO.Id, newProductDTO);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var productEntryDTO = Assert.IsType<ProductEntryOutputDTO>(okObjectResult.Value);
        Assert.Equal(newProductDTO.WorksheetId, productEntryDTO.WorksheetId);
        Assert.NotNull(productEntryDTO.KineticsCellular);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnNotFound404_OnFailure()
    {
        var pricingControllerMocks = new PricingControllerMocks();


        pricingControllerMocks.ProductServiceMock.Setup(p => p.CreateProductAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ProductCreateInputDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new ValidationError("Product Type", "null")));

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var newProductDTO = ProductFixture.TestProductCreateInputDTOKineticsCellular;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        var result = await pricingControllerMocks.PricingController.CreateProduct(testCustomerEntryDTO.Id, testWorksheetDTO.Id, newProductDTO);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Product Type", objectResult.Value.ToString());

    }

    [Fact]
    public async Task GetWorksheetProduct_ShouldReturnOK200_OnSuccess()
    {
        var pricingControllerMocks = new PricingControllerMocks();


        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var newProductDTO = ProductFixture.TestProductCreateInputDTOKineticsCellular;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        pricingControllerMocks.WorksheetServiceMock.Setup(w => w.GetWorksheetProductsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(Result.Ok(new List<ProductEntryOutputDTO>()
        {
            ProductFixture.TestProductEntryDTOKineticsCellular, ProductFixture.TestProductEntryOutputDTOKineticsRoller
        }));

        var result = await pricingControllerMocks.PricingController.GetWorksheetProduct(testCustomerEntryDTO.Id, testWorksheetDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var productEntryDTOList = Assert.IsType<List<ProductEntryOutputDTO>>(okObjectResult.Value);
        Assert.NotEmpty(productEntryDTOList);
        Assert.Equal(testWorksheetDTO.Id, productEntryDTOList[0].WorksheetId);
    }

}