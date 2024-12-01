using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class JsonService<T>
{
    private readonly string _filePath;

    public JsonService(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<List<T>> ReadJsonAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<T>();
        }

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    public async Task WriteJsonAsync(List<T> data)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        var json = JsonSerializer.Serialize(data, options);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
