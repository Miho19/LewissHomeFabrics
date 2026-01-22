using Lewiss.Pricing.Data.Model;
using Microsoft.EntityFrameworkCore;


namespace Lewiss.Pricing.Data.Context;

public class PricingDbContext : DbContext
{
    public PricingDbContext(DbContextOptions<PricingDbContext> options) : base(options) {}

    public DbSet<Worksheet> Worksheet {get; set;}
    public DbSet<Customer> Customer {get; set;}


}