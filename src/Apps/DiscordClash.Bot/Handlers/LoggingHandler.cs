using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace DiscordClash.Bot.Handlers
{
    public class LoggingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger, DiscordSocketClient discord, CommandService commands)
        {
            _logger = logger;

            discord.Log += OnLogAsync;
            commands.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            var logText = $"{msg.Exception?.ToString() ?? msg.Message}";

            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    {
                        _logger.LogCritical("{logText}", logText);
                        break;
                    }
                case LogSeverity.Error:
                    {
                        _logger.LogError("{logText}", logText);
                        break;
                    }
                case LogSeverity.Warning:
                    {
                        _logger.LogWarning("{logText}", logText);
                        break;
                    }
                case LogSeverity.Info:
                    {
                        _logger.LogInformation("{logText}", logText);
                        break;
                    }
                case LogSeverity.Verbose:
                    {
                        _logger.LogTrace("{logText}", logText);
                        break;
                    }
                case LogSeverity.Debug:
                    {
                        _logger.LogDebug("{logText}", logText);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException($"{msg.Severity}");
            }

            return Task.CompletedTask;
        }
    }
}
