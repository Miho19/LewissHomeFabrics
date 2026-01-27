using Lewiss.Pricing.Data.Model;
using Microsoft.EntityFrameworkCore;


namespace Lewiss.Pricing.Data.Context;

public class PricingDbContext : DbContext
{
    public PricingDbContext(DbContextOptions<PricingDbContext> options) : base(options) { }

    public DbSet<Worksheet> Worksheet { get; set; }
    public DbSet<Customer> Customer { get; set; }

    public DbSet<Product> Product { get; set; }

    public DbSet<Option> Option { get; set; }
    public DbSet<OptionVariation> OptionVariation { get; set; }


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


        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().Property(c => c.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Product>().Property(c => c.ExternalMapping);

        modelBuilder.Entity<Product>().HasMany(p => p.OptionVariations).WithMany(ov => ov.Products);

        modelBuilder.Entity<Option>().HasKey(o => o.Id);
        modelBuilder.Entity<Option>().Property(o => o.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<OptionVariation>().HasKey(ov => ov.Id);
        modelBuilder.Entity<OptionVariation>().Property(ov => ov.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Option>().HasMany(o => o.OptionVariation).WithOne(ov => ov.Option).HasForeignKey(ov => ov.OptionId);

        SeedOptions(modelBuilder);

    }


    private void SeedOptions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>().HasData(

        );
    }

}