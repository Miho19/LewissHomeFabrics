using dotenv.net;
using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.Repository.CustomerRepository;
using Lewiss.Pricing.Data.Repository.ProductOptionRepository;
using Lewiss.Pricing.Data.Repository.ProductRepository;
using Lewiss.Pricing.Data.Repository.WorksheetRepository;
using Lewiss.Pricing.Shared.Services;
using Microsoft.EntityFrameworkCore;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = System.Environment.GetEnvironmentVariable("DatabaseConnectionString");

builder.Services.AddDbContext<PricingDbContext>(options =>
{
    options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString));
});

// Repositories
builder.Services.AddScoped<IWorksheetRepository, WorksheetRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductOptionRepository, ProductOptionRepository>();

// Services
builder.Services.AddScoped<PricingService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<WorksheetService>();
builder.Services.AddScoped<FabricService>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
