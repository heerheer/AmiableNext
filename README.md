# Amiable Next

## 是什么？
Amiable Next是使用了HTTPAPI的一个C# Bot开发框架

## 适配那些平台？
目前支持：
- MYQQ

即将支持:
- QQ频道

# 快速上手

## 配置HTTPAPI
[myqqx官网-插件下载文档](https://www.myqqx.net/pages/5e6cb8/#%E5%AE%98%E6%96%B9%E6%8F%90%E4%BE%9B%E7%9A%84httpapi%E6%8F%92%E4%BB%B6)
1. 下载HTTPAPI
2. 配置HTTPAPI插件的信息：[手册](https://www.myqqx.net/pages/198c35/#%E5%9F%BA%E7%A1%80%E9%85%8D%E7%BD%AE)

AmiableNext的回调地址为
`http://host:url/MyqqMsg`

3. 配置AmiableNext的配置文件。位于/config/amiable.next.json


```json
{
    "api_mode": "MYQQ_HTTP_API",
    "api_url": "http://localhost:10002/MyQQHTTPAPI",
    "api_token": "666"
}
```
其中，api_url和api_token都是用于发送请求的。
## 启动AmiableNext
AmiableNext开启后，本身为一个WebAPI。

## 编写事件代码