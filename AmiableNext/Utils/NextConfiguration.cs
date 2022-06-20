using System.Text.Json.Serialization;

namespace AmiableNext.Utils;

public class MiraiConfiguration
{
    public string ApiUrl { get; set; } = "http://localhost:10002/";
    public string AuthToken { get; set; } = "666";
}
public class MyqqConfiguration
{
    public string ApiUrl { get; set; } = "http://localhost:10002/MyQQHTTPAPI";
    public string AuthToken { get; set; } = "666";
}