using Lewiss.Pricing.Data.Context;
using Lewiss.Pricing.Data.FabricData;
using Microsoft.EntityFrameworkCore;

public static class PricingDbContextSeeding
{

    public static async Task SeedDataAsync(PricingDbContext context, CancellationToken cancellationToken = default)
    {
        if (!await context.FabricPrice.AnyAsync(cancellationToken))
        {
            await SeedNewFabricPriceDataAsync(context, cancellationToken);
            return;
        }
    }

    private static async Task SeedNewFabricPriceDataAsync(PricingDbContext context, CancellationToken cancellationToken = default)
    {
        var kineticsLF = FabricPriceDataGenerator.GetPriceModelList(FabricPriceDataGenerator.KineticsRollerLFJSONFile);
        var kineticsSS = FabricPriceDataGenerator.GetPriceModelList(FabricPriceDataGenerator.KineticsRollerSSJSONFile);
        var kineticsTranslucent = FabricPriceDataGenerator.GetPriceModelList(FabricPriceDataGenerator.KineticsCellularTranslucentJSONFile);

        await context.AddRangeAsync([.. kineticsLF, .. kineticsSS, .. kineticsTranslucent], cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}