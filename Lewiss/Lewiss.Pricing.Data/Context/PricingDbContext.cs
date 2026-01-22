using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Lewiss.Pricing.Data.Context;

public class PricingDbContext : DbContext
{
    public PricingDbContext(DbContextOptions<PricingDbContext> options) : base(options) {}

    
}