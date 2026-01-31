using Lewiss.Pricing.Data.FabricData;
using Lewiss.Pricing.Data.Model;
using Lewiss.Pricing.Data.Model.Fabric.Price;
using Lewiss.Pricing.Data.Model.Fabric.Type;
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

    public DbSet<KineticsCellularFabric> KineticsCellularFabric { get; set; }
    public DbSet<KineticsRollerFabric> KineticsRollerFabric { get; set; }

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

        modelBuilder.Entity<KineticsRollerFabric>()
        .HasKey(kr => kr.KineticsRollerFabricId);

        modelBuilder.Entity<KineticsRollerFabric>()
        .Property(kr => kr.KineticsRollerFabricId)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<KineticsRollerFabric>()
        .HasOne(kr => kr.ProductOptionVariation)
        .WithOne()
        .HasForeignKey<KineticsRollerFabric>(kr => kr.ProductOptionVariationId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<KineticsRollerFabric>()
        .HasAlternateKey(f => new { f.Colour, f.Fabric, f.Opacity });

        modelBuilder.Entity<KineticsCellularFabric>()
        .HasAlternateKey(f => new { f.Code, f.Colour, f.Opacity });

        modelBuilder.Entity<KineticsCellularFabric>()
        .HasOne(kr => kr.ProductOptionVariation)
        .WithOne()
        .HasForeignKey<KineticsCellularFabric>(kr => kr.ProductOptionVariationId)
        .OnDelete(DeleteBehavior.Cascade);

        // Fabric Price Data 
        modelBuilder.Entity<FabricPrice>()
        .HasKey(fp => fp.FabricPriceId);

        modelBuilder.Entity<FabricPrice>()
        .Property(fp => fp.FabricPriceId)
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<FabricPrice>()
        .HasAlternateKey(fp => new { fp.Width, fp.Height, fp.Opacity, fp.ProductType });

        // Must come before you add any product option variations to db
        modelBuilder.Entity<ProductOption>().HasData(
            OptionDataUtility.OptionList
        );

        // Seed data here 

        SeedKineticsRollerFabricData(modelBuilder);
        SeedKineticsCellularFabricData(modelBuilder);
        SeedKineticsPricingData(modelBuilder);

        // Called after you have added product option variations with their ids to the OptionVariationList
        modelBuilder.Entity<ProductOptionVariation>().HasData(
            OptionDataUtility.OptionVariationList
        );

    }


    private void SeedKineticsRollerFabricData(ModelBuilder modelBuilder)
    {
        try
        {
            var unLinkedKineticsRollerFabricList = KineticsRollerFabricGenerator.FabricList();
            var unLinkedProductOptionVariationList = KineticsRollerFabricGenerator.GenerateProductOptionVariationList(unLinkedKineticsRollerFabricList);
            var (linkedKineticsRollerFabricList, linkedProductOptionVariationList) = KineticsRollerFabricGenerator.LinkFabricListToProductOptionVariationList(unLinkedKineticsRollerFabricList, unLinkedProductOptionVariationList);

            modelBuilder.Entity<KineticsRollerFabric>()
            .HasData(linkedKineticsRollerFabricList);

            OptionDataUtility.OptionVariationList.AddRange(linkedProductOptionVariationList);

        }

        catch (Exception ex)
        {
            System.Console.WriteLine($"{ex.Message}");
        }


    }

    private void SeedKineticsCellularFabricData(ModelBuilder modelBuilder)
    {
        try
        {
            var unLinkedKineticsCellularFabricList = KineticsCellularFabricGenerator.FabricList();
            var unLinkedProductOptionVariationList = KineticsCellularFabricGenerator.GenerateProductOptionVariationList(unLinkedKineticsCellularFabricList);
            var (linkedKineticsCellularFabricList, linkedProductOptionVariationList) = KineticsCellularFabricGenerator.LinkFabricListToProductOptionVariationList(unLinkedKineticsCellularFabricList, unLinkedProductOptionVariationList);


            modelBuilder.Entity<KineticsCellularFabric>()
            .HasData(linkedKineticsCellularFabricList);

            OptionDataUtility.OptionVariationList.AddRange(linkedProductOptionVariationList);

        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"{ex.Message}");
        }

    }

    private void SeedKineticsPricingData(ModelBuilder modelBuilder)
    {

    }



}