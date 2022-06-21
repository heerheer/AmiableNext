//读取本地配置文件

using System.Runtime.CompilerServices;
using System.Text.Json;
using AmiableNext;
using AmiableNext.SDK;
using AmiableNext.Utils;
using Microsoft.AspNetCore.Http.Json;


var list = AmiableNextApi.String2Mirai("1111111[@123][pic=2333]23232[@22333]").ToList();
Console.WriteLine(JsonSerializer.Serialize(list));
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<AmiableService>().AddHostedService(sp => sp.GetRequiredService<AmiableService>());
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.Configure<MiraiConfiguration>(builder.Configuration.GetSection("Mirai_HTTP_HOOK"));
builder.Services.Configure<MyqqConfiguration>(builder.Configuration.GetSection("MyQQ_HTTP_API"));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Logger.LogWarning("AmiableNext目前依然处于开发中...可能会在后续更新出现破坏性代码变动!");
app.Run();