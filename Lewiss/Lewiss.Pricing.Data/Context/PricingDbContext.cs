using Lewiss.Pricing.Data.Model;
using Microsoft.EntityFrameworkCore;


namespace Lewiss.Pricing.Data.Context;

public class PricingDbContext : DbContext
{
    public PricingDbContext(DbContextOptions<PricingDbContext> options) : base(options) { }

    public DbSet<Worksheet> Worksheet { get; set; }
    public DbSet<Customer> Customer { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>().HasKey(c => c.CustomerId);
        modelBuilder.Entity<Customer>().Property(c => c.CustomerId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Customer>().Property(c => c.ExternalMapping);


        modelBuilder.Entity<Worksheet>().HasKey(w => w.WorksheetId);
        modelBuilder.Entity<Worksheet>().Property(w => w.WorksheetId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Worksheet>().Property(w => w.ExternalMapping);


        modelBuilder.Entity<Worksheet>().HasOne(w => w.Customer).WithMany(c => c.CurrentWorksheets).HasForeignKey(c => c.CustomerId);

        modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
        modelBuilder.Entity<Customer>().HasIndex(c => c.Mobile).IsUnique();

    }

}