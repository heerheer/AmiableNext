using System.Text.Json;
using System.Text.Json.Serialization;

namespace AmiableNext.SDK.Frames;

public class MyQqEventMsgContext
{
    [JsonPropertyName("MQ_robot")] public string MQRobot { get; set; }

    [JsonPropertyName("MQ_type")] public int MQType { get; set; }

    [JsonPropertyName("MQ_type_sub")] public int MQTypeSub { get; set; }

    [JsonPropertyName("MQ_fromID")] public string MQFromID { get; set; }

    [JsonPropertyName("MQ_fromQQ")] public string MQFromQQ { get; set; }

    [JsonPropertyName("MQ_passiveQQ")] public string MQPassiveQQ { get; set; }

    [JsonPropertyName("MQ_msg")] public string MQMsg { get; set; }

    [JsonPropertyName("MQ_msgSeq")] public string MQMsgSeq { get; set; }

    [JsonPropertyName("MQ_msgID")] public string MQMsgID { get; set; }

    [JsonPropertyName("MQ_msgData")] public string MQMsgData { get; set; }

    [JsonPropertyName("MQ_timestamp")] public string MQTimestamp { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

public class WebHookEventContext
{
    public string? UserId { get; set;}
    public string BotId { get; set; }

    /// <summary>
    /// From可能是GroupId，也可能是UserId
    /// </summary>
    public string FromId { get; set;}

    public string Content { get; set;}

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}