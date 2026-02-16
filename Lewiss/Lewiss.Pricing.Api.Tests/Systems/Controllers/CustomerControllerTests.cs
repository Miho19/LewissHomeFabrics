using Castle.Core.Logging;
using FluentResults;
using Lewiss.Pricing.Api.Controllers;
using Lewiss.Pricing.Api.Tests.Fixtures;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Shared.CustomerDTO;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace Lewiss.Pricing.Api.Tests.Systems.Controllers;

public class CustomerControllerTests
{
    private readonly ITestOutputHelper _logger;
    public CustomerControllerTests(ITestOutputHelper testOutputHelper)
    {
        _logger = testOutputHelper;
    }


    [Fact]
    public async Task GetCustomer_ShouldReturnOkWithCustomerEntryDTO_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var customerControllerloggerMock = new Mock<ILogger<CustomerController>>();
        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();

        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);
        var customerController = new CustomerController(customerServiceMock.Object, customerControllerloggerMock.Object);
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.GetCustomerByExternalIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(testCustomerEntryDTO);

        var result = await customerController.GetCustomer(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);

        var returnedCustomerEntryDTO = Assert.IsType<CustomerEntryOutputDTO>(okObjectResult.Value);
        Assert.Equal(testCustomerEntryDTO.FamilyName, returnedCustomerEntryDTO.FamilyName);
        Assert.Equal(testCustomerEntryDTO.Id, returnedCustomerEntryDTO.Id);

    }

    [Fact]
    public async Task GetCustomer_ShouldReturnNotFound404_OnFailure_WhenDatabaseReturnsNull()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var customerControllerloggerMock = new Mock<ILogger<CustomerController>>();
        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();

        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);
        var customerController = new CustomerController(customerServiceMock.Object, customerControllerloggerMock.Object);
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.GetCustomerByExternalIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new NotFoundResource("Customer", testCustomerEntryDTO.Id)));

        var result = await customerController.GetCustomer(testCustomerEntryDTO.Id);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Customer", objectResult.Value.ToString());
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnOkCreated_OnSuccess()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var customerControllerloggerMock = new Mock<ILogger<CustomerController>>();
        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();

        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);
        var customerController = new CustomerController(customerServiceMock.Object, customerControllerloggerMock.Object);
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.CreateCustomerAsync(It.IsAny<CustomerCreateInputDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(testCustomerEntryDTO);
        var result = await customerController.CreateCustomer(CustomerFixture.TestCustomerCreate);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
        var customerEntryDTO = Assert.IsType<CustomerEntryOutputDTO>(createdAtActionResult.Value);
        Assert.Equal(testCustomerEntryDTO.FamilyName, customerEntryDTO.FamilyName);
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturn500_OnFailure()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var customerControllerloggerMock = new Mock<ILogger<CustomerController>>();
        var customerServiceloggerMock = new Mock<ILogger<CustomerService>>();

        var customerServiceMock = new Mock<CustomerService>(unitOfWorkMock.Object, customerServiceloggerMock.Object);
        var customerController = new CustomerController(customerServiceMock.Object, customerControllerloggerMock.Object);
        var testCustomerEntryDTO = CustomerFixture.TestCustomerEntryDTO;

        customerServiceMock.Setup(p => p.CreateCustomerAsync(It.IsAny<CustomerCreateInputDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(new Error("")));
        var result = await customerController.CreateCustomer(CustomerFixture.TestCustomerCreate);

        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Something went wrong", objectResult.Value.ToString());
    }

}
