using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using AmiableNext.SDK;

namespace AmiableNext;

public class AmiableService : AppService
{
    public static AppService app = new();

    public static void Init()
    {
        app.Init();
        app.RegEvent<DemoPrivateMsg>("样例私聊事件");
        app.RegEvent<DemoPrivateMsg2>("样例私聊事件2");
        app.RegEvent<DemoGroupMsg2>("样例群聊事件2");
    }
}

public class DemoPrivateMsg : IBotEvent
{
    public CommonEventType EventType { get; set; } = CommonEventType.MessageFriend;

    public void Process(AmiableEventContext ctx)
    {
        ctx.PrivateReply($"我收到你的私聊消息啦！{ctx.Content}");
    }
}

public class DemoPrivateMsg2 : IBotEvent
{
    public CommonEventType EventType { get; set; } = CommonEventType.MessageFriend;

    public async void Process(AmiableEventContext ctx)
    {
        if (Regex.IsMatch(ctx.Content, @"我是谁[?.]?"))
        {
            ctx.PrivateReply($"你是:{await ctx.Api.GetNickAsync(ctx.MQRobot, ctx.AuthorId)}");
        }
    }
}

public class DemoGroupMsg2 : IBotEvent
{
    public CommonEventType EventType { get; set; } = CommonEventType.MessageGroup;

    public async void Process(AmiableEventContext ctx)
    {
        if (Regex.IsMatch(ctx.Content, @"amiable\.next"))
        {
            var sb = new StringBuilder();
            sb.AppendLine("这是AmiableNext-全新的Amiable哦");
            sb.AppendLine("github: heerheer/AmiableNext");

            ctx.GroupReply(sb.ToString());
        }
    }
}