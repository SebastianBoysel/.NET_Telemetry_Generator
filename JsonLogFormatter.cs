using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using System.Text.Json;

namespace TelemetryGen;

public sealed class JsonLogFormatter : ConsoleFormatter
{
    public JsonLogFormatter() : base("custom-json") { }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        var logObject = new
        {
            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            level = logEntry.LogLevel.ToString(),
            message = logEntry.Formatter(logEntry.State, logEntry.Exception)
        };

        // Create the final object, only including exception if it exists
        object finalLogObject = logEntry.Exception != null 
            ? new
            {
                logObject.timestamp,
                logObject.level,
                logObject.message,
                exception = logEntry.Exception.ToString()
            }
            : logObject;

        var json = JsonSerializer.Serialize(finalLogObject, new JsonSerializerOptions
        {
            WriteIndented = false
        });

        textWriter.WriteLine(json);
    }
}