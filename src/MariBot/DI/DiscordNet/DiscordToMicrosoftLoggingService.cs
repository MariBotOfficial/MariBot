using System.Collections.Concurrent;
using Discord;
using Discord.WebSocket;

namespace MariBot.DI.DiscordNet;

#pragma warning disable CA2254

public class DiscordToMicrosoftLoggingService : IHostedService
{
    private const string BASE_CATEGORY_NAME = "Discord.Net";

    private readonly DiscordSocketClient _client;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ConcurrentDictionary<string, ILogger> _loggers;

    public DiscordToMicrosoftLoggingService(DiscordSocketClient client, ILoggerFactory loggerFactory)
    {
        _client = client;
        _loggerFactory = loggerFactory;

        _loggers = new();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }

        _client.Log += LogAsync;

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }

        _client.Log -= LogAsync;

        return Task.CompletedTask;
    }

    private Task LogAsync(LogMessage arg)
    {
        var categoryName = $"{BASE_CATEGORY_NAME}.{arg.Source}";

        if (!_loggers.TryGetValue(categoryName, out var logger))
        {
            logger = _loggerFactory.CreateLogger(categoryName);
            _ = _loggers.TryAdd(categoryName, logger);
        }

        var logLevel = (LogLevel)Math.Abs((int)arg.Severity - 5);

        logger.Log(logLevel, default, arg.Exception, arg.Message);

        return Task.CompletedTask;
    }
}
