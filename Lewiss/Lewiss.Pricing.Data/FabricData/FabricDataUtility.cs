using System.Text.Json;

namespace Lewiss.Pricing.Data.FabricData;

public static class FabricDataUtility
{
    // public static string FabricDataBaseAddress { get; } = "./FabricData/JSONData";

    public static List<T> GetJSONFileListData<T>(string fileName) where T : struct
    {
        try
        {
            var FabricDataBaseAddress = "/root/lewiss-home-fabrics/Lewiss/Lewiss.Pricing.Data/FabricData/JSONData";


            var filePath = Path.Combine(FabricDataBaseAddress, fileName);
            var jsonString = File.ReadAllText(filePath);

            if (jsonString is null)
            {
                throw new Exception("Failed to retrieve JSON file data");
            }
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var jsonData = JsonSerializer.Deserialize<List<T>>(jsonString, options);
            if (jsonData is null)
            {
                throw new Exception("Failed to deserialise fabric data");
            }

            return jsonData;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to retrieve data:\n{ex.Message}");
        }
    }


}