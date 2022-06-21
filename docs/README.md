# Amiable Next

## 是什么？

Amiable Next是使用了HTTPAPI的一个C# Bot开发框架

## 适配那些平台？

目前支持：

- MYQQ

即将支持:

- QQ频道

# 快速上手

## 1️⃣配置HTTPAPI

### 使用 MYQQ

> 使用MYQQ作为框架，你需要安装MYQQ的官方 HTTP API
>
[myqqx官网-插件下载文档](https://www.myqqx.net/pages/5e6cb8/#%E5%AE%98%E6%96%B9%E6%8F%90%E4%BE%9B%E7%9A%84httpapi%E6%8F%92%E4%BB%B6)

1. 下载HTTPAPI
2. 配置HTTPAPI插件的信息：[手册](https://www.myqqx.net/pages/198c35/#%E5%9F%BA%E7%A1%80%E9%85%8D%E7%BD%AE)

AmiableNext的针对Myqq的回调地址为`/MyqqMsg` (你也可以自己修改)

也就是说，你需要在`appsettings.json`里面配置好本机URL后

在MYQQ内的HTTP API插件配置填入`{url}/MyqqMsg`

同时 配置请求头。

```http request
Content-Type:application/json
```

3. 配置AmiableNext的配置文件。位于同一目录appsettings.json

```json
{
  "Mode": "MyQQ_HTTP_API",
  "MyQQ_HTTP_API": {
    "ApiUrl": "url",
    "AuthToken": "auth"
  }
}
```

### 使用 Mirai-Console-Loader (Mirai)

1. 使用 [mirai-api-http](https://github.com/project-mirai/mirai-api-http)

> AmiableNext仅支持使用2.x的mirai-http-api

2. 配置mirai-http-api

请进入mirai-http-api的配置文件，为其添加http与webhook的adpater

```yaml
adapters: 
  - webhook
  - http
```
紧接着
```
enableVerify: true
verifyKey: xxxxxx
singleMode: true
```

```yaml
adapterSettings:
  webhook:
    destinations:
      - '{{url}}/MiraiEvent' #url应为appsettings.json下配置的地址
  http:
    host: localhost
    port: 8091 # 可更改，但也要同步更改AmiableNext使用的Mirai配置
    cors: [ * ]
```

3. 配置AmiableNext的Mirai兼容

> 位于 appsettings.json

```json
{
  "Mode": "Mirai_HTTP_HOOK",
  "Mirai_HTTP_HOOK": {
    "ApiUrl": "这里是刚刚配置的adapterSettings.host:port",
    "AuthToken": "这里是刚刚自己配置的verifyKey"
  }
}
```

### 支持的API_Mode

- MyQQ_HTTP_API
- Mirai_HTTP_HOOK

位于 `appsettings.json`
```json
{
  "Mode": "Mirai_HTTP_HOOK"
}
```
```json
{
  "Mode": "MyQQ_HTTP_API"
}
```

## 2️⃣启动AmiableNext



## 3️⃣编写事件代码

1. 创建继承IBotEvent的类
2. 实现类。
3. 注册事件。

### 例子

#### 实现类DemoPrivateMsg2

EventType 是 CommonEventType.MessageFriend (好友消息)

```csharp
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
```

#### 注册类

在`AmiableService.cs`中

```csharp
app.RegEvent<DemoPrivateMsg2>("样例私聊事件2");
```

#### 重启应用

重启后成功载入事件。