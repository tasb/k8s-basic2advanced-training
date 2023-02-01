using System.Net;
using Microsoft.Extensions.Logging.Console;
using SwissKnife.API.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddSimpleConsole(opts =>
    {
        opts.IncludeScopes = false;
        opts.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
        opts.ColorBehavior = LoggerColorBehavior.Disabled;
    });

builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    });

builder.Services.AddSingleton<HealthCounter>(new HealthCounter());

var app = builder.Build();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/echo/{message}", (string message) =>
{
    string logMessage = string.Format("Echoing message: {0}", message);
    app.Logger.LogInformation(logMessage);
    return Results.Ok(message);
})
.WithName("Echo");

app.MapGet("/hostname", () =>
{
    var hostname = Dns.GetHostName();
    app.Logger.LogInformation("Getting hostname: {Hostname}", hostname);
    return Results.Ok(hostname);
})
.WithName("Hostname");


app.MapGet("/healthz/start", () =>
{
    app.Logger.LogInformation("Getting startup probe...");
    Thread.Sleep(10000);
    app.Logger.LogInformation("Startup Done!");
    return Results.Ok();
})
.WithName("StartProbe");

app.MapGet("/healthz/ready", (HealthCounter counter) =>
{
    counter.ReadyCount++;
    app.Logger.LogDebug("Ready probe #" + counter.ReadyCount);
    
    if (counter.ReadyCount >= 10 && counter.ReadyCount < 20) {
        app.Logger.LogError("App not ready!");
        return Results.BadRequest();
    } else {
        app.Logger.LogDebug("App ready!");
        return Results.Ok();
    }
})
.WithName("ReadyProbe");

app.MapGet("/healthz/live", (HealthCounter counter) =>
{
    counter.LiveCount++;
    app.Logger.LogDebug("Live probe #" + counter.LiveCount);
    
    if (counter.LiveCount >= 30) {
        app.Logger.LogError("App not running!");
        return Results.BadRequest();
    } else {
        app.Logger.LogDebug("App up & running!");
        return Results.Ok();
    }
})
.WithName("LiveProbe");

app.Run();