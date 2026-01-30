namespace Lewiss.Pricing.Data.FabricData;

public static class FabricDataUtility
{
    public static string FabricDataBaseAddress { get; } = "./FabricData/JSONData";
    public static async Task<string?> GetFileData(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var filePath = Path.Combine(FabricDataBaseAddress, fileName);
            return await File.ReadAllTextAsync(filePath, cancellationToken);
        }
        catch
        {
            return null;
        }
    }
}