using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using AmiableNext.SDK;
using AmiableNext.Utils;

namespace AmiableNext;

// ReSharper disable once ClassNeverInstantiated.Global
public class AmiableService : AppService, IHostedService
{
    public AppService app = new();

    public AmiableNextApi NextApi;

    private readonly ILogger<AmiableService> _logger;
    private readonly IConfiguration _config;

    public AmiableService(ILogger<AmiableService> logger, IHostApplicationLifetime appLifetime, IConfiguration config)
    {
        this._logger = logger;
        _config = config;
        appLifetime.ApplicationStarted.Register(Init);
    }

    public void Init()
    {
        app.Init();
        _logger.LogInformation("Amiable Next 载入事件Instance中...");
        app.RegEvent<DemoPrivateMsg>("样例私聊事件");
        app.RegEvent<DemoPrivateMsg2>("样例私聊事件2");
        app.RegEvent<DemoGroupMsg2>("样例群聊事件2");
        _logger.LogInformation("Amiable Next 载入事件完成");

        var mode = _config.GetValue<string>("Mode");
        var section = _config.GetSection(mode);
        
        _logger.LogInformation("当前模式:{mode}", mode);
        NextApi = new AmiableNextApi(section["ApiUrl"], section["AuthToken"], mode);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{1}\n{2}\n{3}", "Powered By Myqq & Myqq HTTP API!", "Myqq 官网地址 https://www.myqqx.net/",
            "HTTP API插件下载地址 https://daen.lanzoux.com/iEVEk0599isf");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
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
            ctx.PrivateReply($"你是:{await ctx.Api.GetNickAsync(ctx.BotId, ctx.AuthorId)}");
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