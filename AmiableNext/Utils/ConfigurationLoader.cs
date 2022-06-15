using System.Text.Json;
using System.Text.Json.Serialization;

namespace AmiableNext.Utils;

public class ConfigurationLoader
{
    private static readonly ConsoleLogUtil<ConfigurationLoader> _logger = new ConsoleLogUtil<ConfigurationLoader>();

    public static async Task<NextConfiguration> LoadConfig()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "config", "amiable.next.json");
        if (Directory.Exists(Path.GetDirectoryName(path)) is false)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        }

        if (File.Exists(path) is false)
        {
            _logger.Warn($"配置文件不存在...创建{path}");
            File.Create(path).Close();
            File.WriteAllText(path, JsonSerializer.Serialize(new NextConfiguration()));
            return new NextConfiguration();
        }

        return JsonSerializer.Deserialize<NextConfiguration>(await File.ReadAllTextAsync(path)) ?? new();
    }

    public class NextConfiguration
    {
        [JsonPropertyName("api_mode")] public string ApiMode { get; set; } = "MYQQ_HTTP_API";
        [JsonPropertyName("api_url")] public string ApiUrl { get; set; } = "http://localhost:10002/MyQQHTTPAPI";
        [JsonPropertyName("api_token")] public string ApiToken { get; set; } = "666";
    }
}