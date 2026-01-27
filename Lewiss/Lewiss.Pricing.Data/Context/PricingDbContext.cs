using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.OptionData;
using Microsoft.EntityFrameworkCore;


namespace Lewiss.Pricing.Data.Context;

public class PricingDbContext : DbContext
{
    public DbSet<Worksheet> Worksheet { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<ProductOption> ProductOption { get; set; }
    public DbSet<ProductOptionVariation> ProductOptionVariation { get; set; }

    public PricingDbContext(DbContextOptions<PricingDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
        .HasKey(c => c.CustomerId);

        modelBuilder.Entity<Customer>()
        .Property(c => c.CustomerId)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<Customer>()
        .Property(c => c.ExternalMapping);


        modelBuilder.Entity<Worksheet>()
        .HasKey(w => w.WorksheetId);

        modelBuilder.Entity<Worksheet>()
        .Property(w => w.WorksheetId)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<Worksheet>()
        .Property(w => w.ExternalMapping);

        modelBuilder.Entity<Worksheet>()
        .HasOne(w => w.Customer)
        .WithMany(c => c.CurrentWorksheets)
        .HasForeignKey(c => c.CustomerId);

        modelBuilder.Entity<Customer>()
        .HasIndex(c => c.Email).IsUnique();

        modelBuilder.Entity<Customer>()
        .HasIndex(c => c.Mobile).IsUnique();


        modelBuilder.Entity<Product>()
        .HasKey(p => p.ProductId);

        modelBuilder.Entity<Product>()
        .Property(c => c.ProductId).ValueGeneratedOnAdd();

        modelBuilder.Entity<Product>()
        .Property(c => c.ExternalMapping);

        modelBuilder.Entity<Product>()
        .HasMany(p => p.OptionVariations)
        .WithMany(ov => ov.Products);

        modelBuilder.Entity<ProductOption>()
        .HasKey(o => o.ProductOptionId);

        modelBuilder.Entity<ProductOption>()
        .Property(o => o.ProductOptionId)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<ProductOptionVariation>()
        .HasKey(ov => ov.ProductOptionVariationId);

        modelBuilder.Entity<ProductOptionVariation>()
        .Property(ov => ov.ProductOptionVariationId)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<ProductOption>()
        .HasMany(o => o.ProductOptionVariation)
        .WithOne(ov => ov.ProductOption)
        .HasForeignKey(ov => ov.ProductOptionId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductOption>().HasData(
            OptionDataUtility.OptionList
        );

        modelBuilder.Entity<ProductOptionVariation>().HasData(
            OptionDataUtility.OptionVariationList
        );

    }



}