using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.FabricData;
using Microsoft.EntityFrameworkCore;

public static class PricingDbContextSeeding
{
    public static void SeedData(PricingDbContext context, bool anyStoreManagementOperationPerformed)
    {
        if (!context.FabricPrice.Any())
        {
            SeedNewFabricPriceData(context, anyStoreManagementOperationPerformed);
            return;
        }

        // To add later; for when we want to update existing pricing
    }

    private static void SeedNewFabricPriceData(PricingDbContext context, bool anyStoreManagementOperationPerformed)
    {

        var kineticsLF = FabricPriceDataGenerator.GetPriceModelList(FabricPriceDataGenerator.KineticsRollerLFJSONFile);
        var kineticsSS = FabricPriceDataGenerator.GetPriceModelList(FabricPriceDataGenerator.KineticsRollerSSJSONFile);
        var kineticsTranslucent = FabricPriceDataGenerator.GetPriceModelList(FabricPriceDataGenerator.KineticsCellularTranslucentJSONFile);

        context.AddRange([.. kineticsLF, .. kineticsSS, .. kineticsTranslucent]);
        context.SaveChanges();
    }

    public static async Task SeedDataAsync(PricingDbContext context, bool anyStoreManagementOperationPerformed, CancellationToken cancellationToken = default)
    {
        if (!await context.FabricPrice.AnyAsync(cancellationToken))
        {
            await SeedNewFabricPriceDataAsync(context, anyStoreManagementOperationPerformed, cancellationToken);
            return;
        }
    }

    private static async Task SeedNewFabricPriceDataAsync(PricingDbContext context, bool anyStoreManagementOperationPerformed, CancellationToken cancellationToken = default)
    {
        var kineticsLF = FabricPriceDataGenerator.GetPriceModelList(FabricPriceDataGenerator.KineticsRollerLFJSONFile);
        var kineticsSS = FabricPriceDataGenerator.GetPriceModelList(FabricPriceDataGenerator.KineticsRollerSSJSONFile);
        var kineticsTranslucent = FabricPriceDataGenerator.GetPriceModelList(FabricPriceDataGenerator.KineticsCellularTranslucentJSONFile);

        await context.AddRangeAsync([.. kineticsLF, .. kineticsSS, .. kineticsTranslucent], cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}