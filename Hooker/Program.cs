using Hooker.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

var builder = Host.CreateApplicationBuilder();

builder.Logging.ClearProviders();
builder.Logging.AddDebug();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "[HH:mm:ss.fff] ";
    options.ColorBehavior = LoggerColorBehavior.Enabled;
});

builder.Services.AddHookerCore();

var app = builder.Build();

await app.RunAsync();
