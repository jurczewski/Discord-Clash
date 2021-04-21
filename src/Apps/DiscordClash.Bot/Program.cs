using Cocona;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordClash.Bot.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DiscordClash.Bot.Services;

namespace DiscordClash.Bot
{
    class Program
    {
        private readonly ILogger<Program> _logger;

        public Program(ILogger<Program> logger)
        {
            _logger = logger;
        }

        static async Task Main(string[] args) =>
            await CoconaApp.Create()
                .UseLogger("DiscordClash.Bot")
                .ConfigureServices((ctx, services) =>
                {
                    var configuration = ctx.Configuration;
                    services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Verbose,
                        MessageCacheSize = 1000,
                        AlwaysDownloadUsers = true
                    }))
                    .AddSingleton(new CommandService(new CommandServiceConfig
                    {
                        LogLevel = LogSeverity.Verbose,
                        DefaultRunMode = RunMode.Async,
                    }))
                    .AddSingleton<CommandHandler>()
                    //.AddSingleton<StartupService>()
                    //.AddSingleton<LoggingService>()
                    ;
                })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("appsettings.json", true, true)
                        .AddEnvironmentVariables();
                })
                .RunAsync<Program>(args);
    }
}
