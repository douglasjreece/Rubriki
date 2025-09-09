using System.Text.Json;

namespace Rubriki.Cqrs;

public class StorageQuery(StorageOptions options) : IStorageQuery
{
    public async Task<T?> GetOrDefault<T>(string fileName) where T : class
    {
        try
        {
            var filePath = Path.Combine(options.AppDataDirectory, fileName);
            var jsonText = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T>(jsonText);
        }
        catch
        {
            return null;
        }
    }
}
