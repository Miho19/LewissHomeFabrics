using Lewiss.Pricing.Data.Model;
using Microsoft.EntityFrameworkCore;


namespace Lewiss.Pricing.Data.Context;

public class PricingDbContext : DbContext
{
    public PricingDbContext(DbContextOptions<PricingDbContext> options) : base(options) {}

    public DbSet<Worksheet> Worksheet {get; set;}
    public DbSet<Customer> Customer {get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>().HasKey(c => c.Id);
        modelBuilder.Entity<Worksheet>().HasKey(w => w.Id);
        modelBuilder.Entity<Customer>().HasMany(c => c.CurrentWorksheets).WithOne(w => w.Customer).HasForeignKey(w => w.CustomerId);

    }


}