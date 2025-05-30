using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TelemetryGen;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Debug);
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<ConsoleFormatter, JsonLogFormatter>();
    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole(options =>
        {
            options.FormatterName = "custom-json";
        });
    });

using var host = builder.Build();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

var messages = new (LogLevel Level, string Message)[]
{
    (LogLevel.Debug, "DEBUG - this is a debug log"),
    (LogLevel.Information, "INFORMATION - information level log"),
    (LogLevel.Warning, "WARNING - warning, something went wrong"),
    (LogLevel.Error, "ERROR - error, something bad happened"),
    (LogLevel.Critical, "CRITICAL - something died")
};

int i = 0;
while (true)
{
    var (level, message) = messages[i % messages.Length];
    logger.Log(level, message);
    i++;
    await Task.Delay(TimeSpan.FromSeconds(10));
}