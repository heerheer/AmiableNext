using System.Runtime.Serialization;
using System.Text.Json;
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
    private readonly ConsoleLogUtil<MyqqMsgController> _logger;

    public MyqqMsgController()
    {
        _logger = new();
    }

    [HttpPost]
    public string OnMsg([FromBody] MyQqEventMsgContext context)
    {
        EventHandleResult HandleResult = EventHandleResult.Continue;
#if DEBUG
        _logger.Info(context.ToString());
#endif

        context.MQMsg = System.Web.HttpUtility.UrlDecode(context.MQMsg) ?? "";

        var ctx = new AmiableEventContext(Manager.NextApi);

        foreach (var property in typeof(MyQqEventMsgContext).GetProperties())
        {
            if (property.CanRead && property.CanWrite)
            {
                property.SetValue(ctx, property.GetValue(context, null), null);
            }
        }


        //在这里，会分发事件
        AmiableService.app.Invoke(context.MQType, ctx);

        return $"{{\"status\":{(int)HandleResult}}}";
    }
}