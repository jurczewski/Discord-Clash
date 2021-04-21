using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordClash.Bot.Infrastructure;
using DiscordClash.Bot.Services;
using Figgle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordClash.Bot
{
    public class Program
    {
        private static string ApplicationName => Assembly.GetCallingAssembly().GetName().Name;

        public static async Task Main()
        {
            DisplayBanner();

            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Logging.SetupSerilog(ApplicationName);

            Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureServices(async (ctx, services) =>
                {
                    var configuration = ctx.Configuration;
                    services.Configure<BotSettings>(configuration.GetSection("BotSettings"));

                    ConfigureServices(services);

                    var provider = services.BuildServiceProvider();
                    PrepareServiceProvider(provider);

                    await provider.GetRequiredService<StartupService>().StartAsync();
                })
                .Build();

            // Delay indefinitely.
            await Task.Delay(Timeout.Infinite);
        }

        private static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
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
            .AddSingleton<LoggingService>()
            .AddSingleton<CommandHandler>()
            .AddSingleton<StartupService>();
        }

        private static void PrepareServiceProvider(ServiceProvider provider)
        {
            provider.GetRequiredService<LoggingService>();
            provider.GetRequiredService<CommandHandler>();
        }

        private static void DisplayBanner()
        {
            Console.WriteLine(FiggleFonts.Doom.Render(ApplicationName));
        }
    }
}
