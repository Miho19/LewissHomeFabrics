using Lewiss.Pricing.Data.FabricData;
using Lewiss.Pricing.Data.Model.Fabric;

public static class KineticsCellularFabricGenerator
{
    public static string KineticsCellularJSONFabricDataFileName { get; } = "KineticsCellularFabricData.json";
    public static async Task<List<KineticsCellularFabric>> FabricListAsync(CancellationToken cancellationToken = default)
    {
        var jsonString = FabricDataUtility.GetFileData(KineticsCellularJSONFabricDataFileName, cancellationToken);
        if (jsonString is null)
        {
            throw new Exception("Failed to retrieve Kinetics Cellular Fabric JSON file");
        }


        return [];
    }
}