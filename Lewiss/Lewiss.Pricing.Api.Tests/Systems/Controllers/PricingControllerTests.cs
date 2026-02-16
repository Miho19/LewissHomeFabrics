using Castle.Core.Logging;
using FluentResults;
using Lewiss.Pricing.Api.Controllers;
using Lewiss.Pricing.Api.Tests.Fixtures;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.ProductDTO;
using Lewiss.Pricing.Shared.ProductStrategy;
using Lewiss.Pricing.Shared.QueryParameters;
using Lewiss.Pricing.Shared.Services;
using Lewiss.Pricing.Shared.WorksheetDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);

        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);


        var productServiceLoggerMock = new Mock<ILogger<ProductService>>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, fabricServiceMock.Object, productStrategyResolverMock.Object, productServiceLoggerMock.Object);

        var worksheetServiceLoggerMock = new Mock<ILogger<WorksheetService>>();
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, worksheetServiceLoggerMock.Object);

        var pricingController = new PricingController(customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        worksheetServiceMock.Setup(p => p.CreateWorksheetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(WorksheetFixture.TestWorksheetDTO));

        var result = await pricingController.CreateWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);

        var worksheetDTO = Assert.IsType<WorksheetOutputDTO>(createdAtActionResult.Value);
        Assert.Equal(testCustomerEntryDTO.Id, worksheetDTO.CustomerId);

    }

    [Fact]
    public async Task CreateWorksheet_ShouldReturnStatus500_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);

        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);


        var productServiceLoggerMock = new Mock<ILogger<ProductService>>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, fabricServiceMock.Object, productStrategyResolverMock.Object, productServiceLoggerMock.Object);

        var worksheetServiceLoggerMock = new Mock<ILogger<WorksheetService>>();
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, worksheetServiceLoggerMock.Object);

        var pricingController = new PricingController(customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);


        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        worksheetServiceMock.Setup(p => p.CreateWorksheetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new NotFoundResource("Customer", testCustomerEntryDTO.Id)));

        var result = await pricingController.CreateWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Customer", objectResult.Value.ToString());
    }






    [Fact]
    public async Task GetWorksheet_ShouldReturnOK200_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);

        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);


        var productServiceLoggerMock = new Mock<ILogger<ProductService>>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, fabricServiceMock.Object, productStrategyResolverMock.Object, productServiceLoggerMock.Object);

        var worksheetServiceLoggerMock = new Mock<ILogger<WorksheetService>>();
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, worksheetServiceLoggerMock.Object);

        var pricingController = new PricingController(customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        worksheetServiceMock.Setup(p => p.GetWorksheetAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(testWorksheetDTO));

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        var result = await pricingController.GetWorksheet(testCustomerEntryDTO.Id, testWorksheetDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var worksheetDTO = Assert.IsType<WorksheetOutputDTO>(okObjectResult.Value);
        Assert.Equal(testWorksheetDTO.Id, worksheetDTO.Id);
    }

    [Fact]
    public async Task GetWorksheet_ShouldReturn500_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);

        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);


        var productServiceLoggerMock = new Mock<ILogger<ProductService>>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, fabricServiceMock.Object, productStrategyResolverMock.Object, productServiceLoggerMock.Object);

        var worksheetServiceLoggerMock = new Mock<ILogger<WorksheetService>>();
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, worksheetServiceLoggerMock.Object);

        var pricingController = new PricingController(customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;

        worksheetServiceMock.Setup(p => p.GetWorksheetAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new NotFoundResource("Worksheet", testWorksheetDTO.Id)));

        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;
        var result = await pricingController.GetWorksheet(testCustomerEntryDTO.Id, testWorksheetDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Worksheet", objectResult.Value.ToString());
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturnOK200_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);

        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);


        var productServiceLoggerMock = new Mock<ILogger<ProductService>>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, fabricServiceMock.Object, productStrategyResolverMock.Object, productServiceLoggerMock.Object);

        var worksheetServiceLoggerMock = new Mock<ILogger<WorksheetService>>();
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, worksheetServiceLoggerMock.Object);

        var pricingController = new PricingController(customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);


        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(new List<WorksheetOutputDTO>
        {
            testWorksheetDTO
        }));

        var result = await pricingController.GetCustomerWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var worksheetDTOList = Assert.IsType<List<WorksheetOutputDTO>>(okObjectResult.Value);
        Assert.NotEmpty(worksheetDTOList);
        Assert.Equal(testWorksheetDTO.Id, worksheetDTOList[0].Id);
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturn500_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);

        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);


        var productServiceLoggerMock = new Mock<ILogger<ProductService>>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, fabricServiceMock.Object, productStrategyResolverMock.Object, productServiceLoggerMock.Object);

        var worksheetServiceLoggerMock = new Mock<ILogger<WorksheetService>>();
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, worksheetServiceLoggerMock.Object);

        var pricingController = new PricingController(customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new NotFoundResource("Customer", testCustomerEntryDTO.Id)));


        var result = await pricingController.GetCustomerWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Customer", objectResult.Value.ToString());
    }

    [Fact]
    public async Task GetCustomerWorksheet_ShouldReturn200Ok_OnSuccess_WhenEmptyListIsReturned()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);

        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);


        var productServiceLoggerMock = new Mock<ILogger<ProductService>>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, fabricServiceMock.Object, productStrategyResolverMock.Object, productServiceLoggerMock.Object);

        var worksheetServiceLoggerMock = new Mock<ILogger<WorksheetService>>();
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, worksheetServiceLoggerMock.Object);

        var pricingController = new PricingController(customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.GetCustomerWorksheetDTOListAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(new List<WorksheetOutputDTO>()));

        var result = await pricingController.GetCustomerWorksheet(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        var workoutDTOList = Assert.IsType<List<WorksheetOutputDTO>>(okObjectResult.Value);
        Assert.Empty(workoutDTOList);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnOK200_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);

        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);


        var productServiceLoggerMock = new Mock<ILogger<ProductService>>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, fabricServiceMock.Object, productStrategyResolverMock.Object, productServiceLoggerMock.Object);

        var worksheetServiceLoggerMock = new Mock<ILogger<WorksheetService>>();
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, worksheetServiceLoggerMock.Object);

        var pricingController = new PricingController(customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        productServiceMock.Setup(p => p.CreateProductAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ProductCreateInputDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(ProductFixture.TestProductEntryDTOKineticsCellular));

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var newProductDTO = ProductFixture.TestProductCreateInputDTOKineticsCellular;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        var result = await pricingController.CreateProduct(testCustomerEntryDTO.Id, testWorksheetDTO.Id, newProductDTO);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var productEntryDTO = Assert.IsType<ProductEntryOutputDTO>(okObjectResult.Value);
        Assert.Equal(newProductDTO.WorksheetId, productEntryDTO.WorksheetId);
        Assert.NotNull(productEntryDTO.KineticsCellular);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnNotFound404_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);

        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);


        var productServiceLoggerMock = new Mock<ILogger<ProductService>>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, fabricServiceMock.Object, productStrategyResolverMock.Object, productServiceLoggerMock.Object);

        var worksheetServiceLoggerMock = new Mock<ILogger<WorksheetService>>();
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, worksheetServiceLoggerMock.Object);

        var pricingController = new PricingController(customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        productServiceMock.Setup(p => p.CreateProductAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ProductCreateInputDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new ValidationError("Product Type", "null")));

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var newProductDTO = ProductFixture.TestProductCreateInputDTOKineticsCellular;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        var result = await pricingController.CreateProduct(testCustomerEntryDTO.Id, testWorksheetDTO.Id, newProductDTO);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Product Type", objectResult.Value.ToString());

    }

    [Fact]
    public async Task GetWorksheetProduct_ShouldReturnOK200_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();
        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);

        var sharedUtilityServiceMock = new Mock<SharedUtilityService>(unitOfWorkMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var productStrategyResolverMock = new Mock<ProductStrategyResolver>(serviceProviderMock.Object);

        var fabricServiceLoggerMock = new Mock<ILogger<FabricService>>();
        var fabricServiceMock = new Mock<FabricService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, fabricServiceLoggerMock.Object);


        var productServiceLoggerMock = new Mock<ILogger<ProductService>>();
        var productServiceMock = new Mock<ProductService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, fabricServiceMock.Object, productStrategyResolverMock.Object, productServiceLoggerMock.Object);

        var worksheetServiceLoggerMock = new Mock<ILogger<WorksheetService>>();
        var worksheetServiceMock = new Mock<WorksheetService>(unitOfWorkMock.Object, sharedUtilityServiceMock.Object, productStrategyResolverMock.Object, worksheetServiceLoggerMock.Object);

        var pricingController = new PricingController(customerServiceMock.Object, productServiceMock.Object, worksheetServiceMock.Object);

        var testWorksheetDTO = WorksheetFixture.TestWorksheetDTO;
        var newProductDTO = ProductFixture.TestProductCreateInputDTOKineticsCellular;
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        worksheetServiceMock.Setup(w => w.GetWorksheetProductsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(Result.Ok(new List<ProductEntryOutputDTO>()
        {
            ProductFixture.TestProductEntryDTOKineticsCellular, ProductFixture.TestProductEntryOutputDTOKineticsRoller
        }));

        var result = await pricingController.GetWorksheetProduct(testCustomerEntryDTO.Id, testWorksheetDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var productEntryDTOList = Assert.IsType<List<ProductEntryOutputDTO>>(okObjectResult.Value);
        Assert.NotEmpty(productEntryDTOList);
        Assert.Equal(testWorksheetDTO.Id, productEntryDTOList[0].WorksheetId);
    }

}