using System.Runtime.CompilerServices;
using AmiableNext.SDK.Enums;
using AmiableNext.SDK.Frames;

namespace AmiableNext.SDK
{
    /// <summary>
    /// 最基础的事件参数集 遵循OneBot标准
    /// </summary>
    public class AmiableEventContext : WebHookEventContext
    {
        public AmiableEventContext(AmiableNextApi api)
        {
            Api = api;
        }

        public string AuthorId =>
            UserId;

        public string GroupId =>
            FromId;

        public EventHandleResult HandleResult { get; set; } = EventHandleResult.Neglect;

        public AmiableNextApi Api;
    }


    public static class MessageApiExtension
    {
        /// <summary>
        /// 好友私聊回复
        /// </summary>
        /// <param name="content"></param>
        public static void PrivateReply(this AmiableEventContext ctx, string content)
        {
            ctx.Api.SendPrivateMsg(ctx.BotId, ctx.GroupId, content);
        }

        /// <summary>
        /// 群聊回复
        /// </summary>
        /// <param name="content"></param>
        public static void GroupReply(this AmiableEventContext ctx, string content)
        {
            ctx.Api.SendGroupMsg(ctx.BotId, ctx.GroupId, content);
        }
    }
}