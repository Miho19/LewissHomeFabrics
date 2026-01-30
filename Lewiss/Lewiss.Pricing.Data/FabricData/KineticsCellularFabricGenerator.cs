using System.Text.Json;
using Lewiss.Pricing.Data.FabricData;
using Lewiss.Pricing.Data.Model.Fabric;

public static class KineticsCellularFabricGenerator
{

    private record KineticsCellularFabricDataJSONStructure()
    {
        public required string Code;
        public required string Colour;
        public required string Opacity;
        public required int Multiplier;

    }

    public static string KineticsCellularJSONFabricDataFileName { get; } = "KineticsCellularFabricData.json";
    public static async Task<List<KineticsCellularFabric>> FabricListAsync(CancellationToken cancellationToken = default)
    {
        var jsonString = await FabricDataUtility.GetFileData(KineticsCellularJSONFabricDataFileName, cancellationToken);
        if (jsonString is null)
        {
            throw new Exception("Failed to retrieve Kinetics Cellular Fabric JSON file");
        }

        var jsonData = JsonSerializer.Deserialize<List<KineticsCellularFabricDataJSONStructure>>(jsonString);
        if (jsonData is null)
        {
            throw new Exception("Failed to deserialise Kinetics Cellular Fabric data");
        }

        jsonData.Select(f => );

        return [];
    }
}