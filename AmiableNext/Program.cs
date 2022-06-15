//读取本地配置文件

using System.Runtime.CompilerServices;
using AmiableNext;
using AmiableNext.SDK;
using AmiableNext.Utils;



var _logger = new ConsoleLogUtil<Program>();

_logger.Duang("Powered By Myqq & Myqq HTTP API!");
_logger.Duang("Myqq 官网地址 https://www.myqqx.net/");
_logger.Duang("HTTP API插件下载地址 https://daen.lanzoux.com/iEVEk0599isf");


ConfigurationLoader.NextConfiguration config = await ConfigurationLoader.LoadConfig();
_logger.Info("读取配置文件完成。");

Manager.NextApi = new AmiableNextApi(config.ApiUrl, config.ApiToken);

AmiableService.Init();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.Run();

public static class Manager
{
    public static AmiableNextApi NextApi;
}