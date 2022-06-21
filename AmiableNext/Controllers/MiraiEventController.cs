using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using AmiableNext.SDK;
using AmiableNext.SDK.Enums;
using AmiableNext.SDK.Frames;
using AmiableNext.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AmiableNext.Controllers;

[ApiController]
[Route("[controller]")]
public class MiraiEventController : ControllerBase
{
    private readonly ILogger<MiraiEventController> _logger;
    private readonly AmiableService _amiable;

    public MiraiEventController(ILogger<MiraiEventController> _logger, AmiableService amiable)
    {
        this._logger = _logger;
        this._amiable = amiable;
    }

    [HttpPost]
    public void OnMsg([FromBody] JsonNode originContext)
    {
        string? GetGroup()
        {
            if (originContext["group"] != null)
            {
                return originContext["group"]["id"].ToString()!;
            }

            if (originContext["sender"] != null)
            {
                return originContext["sender"]!["group"]?["id"]?.ToString();
            }

            return null;
        }

        string? GetUser()
        {
            return originContext["sender"]?["id"]?.ToString();
        }

        string ConvertMessage()
        {
            var sb = new StringBuilder();
            var list = originContext["messageChain"]?.AsArray();
            if (list == null)
                return "";
            list.ToList().ForEach(x =>
            {
                switch (x["type"]!.ToString())
                {
                    case "Source":
                        break;
                    case "Plain":
                        sb.Append(x["text"]!);
                        break;
                    case "Image":
                        sb.Append($"[pic={x["imageId"]}]");
                        break;
                }
            });
            return sb.ToString();
        }


        if (GetGroup() != null)
        {
            if (GetGroup() != "906831921")
            {
                Console.WriteLine("re");
                return;
            }
        }

        EventHandleResult HandleResult = EventHandleResult.Continue;
        
        _logger.LogInformation("{ori}", originContext.ToString());


        var ctx = new AmiableEventContext(_amiable.NextApi)
        {
            BotId = HttpContext.Request.Headers["qq"].ToString(),
            FromId = GetGroup() ?? GetUser()!,
            UserId = GetUser(),
            Content = ConvertMessage()
        };

#if DEBUG
        _logger.LogDebug("{ctx}", ctx.ToString());
#endif

        var type = originContext["type"].ToString() switch
        {
            "GroupMessage" => (int)CommonEventType.MessageGroup,
            _ => -1,
        };

        //在这里，会分发事件
        _amiable.app.Invoke(type, ctx);

        this.HttpContext.Response.StatusCode = 200;
    }
}