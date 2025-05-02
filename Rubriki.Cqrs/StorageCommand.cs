using System.Text.Json;

namespace Rubriki.Cqrs;

public class StorageCommand(StorageOptions options)
{
    public async Task Store<T>(string fileName, T content) where T : class
    {
        var filePath = Path.Combine(options.AppDataDirectory, fileName);
        var jsonText = JsonSerializer.Serialize(content);
        await File.WriteAllTextAsync(filePath, jsonText);
    }

    public async Task Remove(string fileName)
    {
        await Task.Run(() =>
        {
            var filePath = Path.Combine(options.AppDataDirectory, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        });
    }
}
