using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DiscordClash.Bot.Services
{
    public class LoggingService //todo: rename all 'services' to handlers
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger, DiscordSocketClient discord, CommandService commands)
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
