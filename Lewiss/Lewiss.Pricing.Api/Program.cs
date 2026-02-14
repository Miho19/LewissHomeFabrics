using dotenv.net;
using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Repository.CustomerRepository;
using Lewiss.Pricing.Data.Repository.Fabric;
using Lewiss.Pricing.Data.Repository.ProductOptionRepository;
using Lewiss.Pricing.Data.Repository.ProductRepository;
using Lewiss.Pricing.Data.Repository.WorksheetRepository;
using Lewiss.Pricing.Shared.CustomError;
using Lewiss.Pricing.Shared.ProductStrategy;
using Lewiss.Pricing.Shared.Services;
using Microsoft.EntityFrameworkCore;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = System.Environment.GetEnvironmentVariable("SQLServerDatabaseConnectionString");

if (string.IsNullOrEmpty(dbConnectionString))
{
    throw new InvalidOperationException("Database Connection string is null");
}

builder.Services.AddDbContext<PricingDbContext>(options =>
{
    options.UseSqlServer(connectionString: dbConnectionString);
});


builder.Services.AddProblemDetails(configure =>
{
    configure.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


// Repositories
builder.Services.AddScoped<IWorksheetRepository, WorksheetRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductOptionRepository, ProductOptionRepository>();
builder.Services.AddScoped<IKineticsCellularFabricRepository, KineticsCellularFabricRepository>();
builder.Services.AddScoped<IKineticsRollerFabricRepository, KineticsRollerFabricRepository>();
builder.Services.AddScoped<IFabricPriceRepository, FabricPriceRepository>();






// Services
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<WorksheetService>();
builder.Services.AddScoped<FabricService>();
builder.Services.AddScoped<SharedUtilityService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Product strategy patterns
builder.Services.AddScoped<ProductStrategyResolver>();
builder.Services.AddScoped<KineticsRollerProductStrategy>();
builder.Services.AddScoped<KineticsCellularProductStrategy>();



builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
