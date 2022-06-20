using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using AmiableNext.SDK;
using AmiableNext.SDK.Enums;
using AmiableNext.SDK.Frames;
using AmiableNext.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AmiableNext.Controllers;

[ApiController]
[Route("[controller]")]
public class MyqqMsgController : ControllerBase
{
    private readonly ILogger<MyqqMsgController> _logger;

    public MyqqMsgController(ILogger<MyqqMsgController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public string OnMsg([FromBody] JsonNode context)
    {
        EventHandleResult HandleResult = EventHandleResult.Continue;

#if DEBUG
        _logger.LogInformation("{ctx}", context.ToString());
#endif

        context["MQ_Msg"] = System.Web.HttpUtility.UrlDecode(context["MQ_Msg"]!.ToString()) ?? "";

        //var ctx = new AmiableEventContext(Manager.NextApi);

        //在这里，会分发事件
        // AmiableService.app.Invoke(context.MQ_Type, ctx);

         return $"{{\"status\":{(int)HandleResult}}}";
    }
}