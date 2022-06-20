using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace AmiableNext.SDK;

public class AmiableNextApi
{
    /*
     * 参考 https://www.myqqx.net/pages/0afae9/
     */

    private HttpClient _httpClient;
    private readonly string _token;
    private readonly string _mode;

    public AmiableNextApi(string url, string token, string mode)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(url);
        _token = token;
        _mode = mode;

        if (mode == "Mirai_HTTP_HOOK")
        {
            Verify();
        }
    }

    private async void Verify()
    {
        var resp = await SendMiraiReq("/verify", new
        {
            verifyKey = _token
        });

        if (resp != null)
        {
            Console.WriteLine(resp["code"]);
        }
    }

    /// <summary>
    /// 发送群消息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="group"></param>
    /// <param name="content"></param>
    public void SendGroupMsg(string bot, string group, string content)
    {
        SendMessage(bot, group, null, content);
    }

    /// <summary>
    /// 取QQ昵称
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="qq"></param>
    /// <returns></returns>
    public async Task<string> GetNickAsync(string bot, string qq)
    {
        var body = CreateBody("Api_GetNick");
        body.SetParams(bot, qq);
        var result = await SendApiReq(body);
        return result.Data.Ret;
    }

    /// <summary>
    /// 取生日
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="qq"></param>
    /// <returns></returns>
    public async Task<String> GetBirthdayAsync(string bot, string qq)
    {
        var body = CreateBody("Api_GetNick");
        body.SetParams(bot, qq);
        var result = await SendApiReq(body);
        return result.Data.Ret;
    }

    /// <summary>
    /// 取群名片
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="qq"></param>
    /// <returns></returns>
    public async Task<string> GetGroupCardAsync(string bot, string qq)
    {
        var body = CreateBody("Api_GetGroupCard");
        body.SetParams(bot, qq);
        var result = await SendApiReq(body);
        return result.Data.Ret;
    }


    /// <summary>
    /// 发送私聊消息
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="qq"></param>
    /// <param name="content"></param>
    public void SendPrivateMsg(string bot, string qq, string content)
    {
        SendMessage(bot, null, qq, content);
    }

    private void SendMessage(string bot, string? group, string? qq, string content)
    {
        int type = 0;
        if (string.IsNullOrEmpty(group)) type = 1;

        if (string.IsNullOrEmpty(qq)) type = 2;

        if (type == 0)
        {
            throw new("group与qq参数异常");
        }

        if (_mode == "Mirai_HTTP_HOOK")
        {
            if (type == 1)
            {
                SendMiraiReq("/sendFriendMessage", new
                {
                    sessionKey = _token,
                    target = qq,
                    messageChain = String2Mirai(content)
                });
            }

            if (type == 2)
            {

                SendMiraiReq("/sendGroupMessage", new
                {
                    sessionKey = "",
                    target = group,
                    messageChain = String2Mirai(content)
                });
            }
        }

        if (_mode == "MyQQ_HTTP_API")
        {
            var body = CreateBody("Api_SendMsg");
            body.SetParams(bot, type, group, qq, content);
            //发送请求
            SendApiReq(body);
        }
    }

    private async Task<MyqqApiResult?> SendApiReq(ApiBody body)
    {
        var content = new StringContent(JsonSerializer.Serialize(body));
        return JsonSerializer.Deserialize<MyqqApiResult>(await (await _httpClient.PostAsync("", content)).Content
            .ReadAsStringAsync());
    }

    private async Task<JsonNode?> SendMiraiReq(string api, dynamic body)
    {
        Console.WriteLine("send");
        var content = new StringContent(JsonSerializer.Serialize(body));
        var resp = JsonSerializer.Deserialize<JsonNode>(await (await _httpClient.PostAsync(api, content)).Content
            .ReadAsStringAsync());
        Console.WriteLine(resp.ToString());
        return resp;
    }

    /// <summary>
    /// 创建一个请求体
    /// </summary>
    /// <param name="function"></param>
    /// <returns></returns>
    private ApiBody CreateBody(string function)
    {
        return new ApiBody() { Function = function, Token = _token };
    }

    /// <summary>
    /// POST Api请求体
    /// </summary>
    class ApiBody
    {
        [JsonPropertyName("function")] public string Function { get; set; }
        [JsonPropertyName("token")] public string Token { get; set; }
        [JsonPropertyName("params")] public Dictionary<string, string> Params { get; set; }

        public void SetParams(params object?[] args)
        {
            Params = new Dictionary<string, string>();
            for (var index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                Params.Add($"c{index + 1}", arg?.ToString() ?? "");
            }
        }
    }

    public class MyqqApiResultData
    {
        [JsonPropertyName("ret")] public string Ret { get; set; }
    }

    class MyqqApiResult
    {
        [JsonPropertyName("success")] public bool Success { get; set; }

        [JsonPropertyName("code")] public int Code { get; set; }

        [JsonPropertyName("msg")] public string Msg { get; set; }

        [JsonPropertyName("data")] public MyqqApiResultData Data { get; set; }
    }

    IEnumerable<object> String2Mirai(string content)
    {
        return new[]
        {
            new
            {
                type = "Plain",
                text = content
            }
        };
    }
}