using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordClash.Application.BotHelpers;
using DiscordClash.Application.UseCases;
using DiscordClash.Bot.Infrastructure;
using DiscordClash.Bot.Services;
using EasyNetQ;
using Figgle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordClash.Bot
{
    public class Program
    {
        private static IConfigurationRoot Configuration { get; set; }

        public static async Task Main()
        {
            DisplayBanner();

            Host.CreateDefaultBuilder()
                .UseCustomLogging()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("appsettings.json", true, true)
                        .AddEnvironmentVariables();
                    Configuration = builder.Build();
                })
                .ConfigureServices(async (_, services) =>
                {
                    ConfigureServices(services);

                    var provider = services.BuildServiceProvider();
                    provider.GetRequiredService<LoggingService>();
                    provider.GetRequiredService<CommandHandler>();

                    await provider.GetRequiredService<StartupService>().StartAsync();

                    var msgService = provider.GetService<MessageService>();
                    msgService?.ProcessMessages();
                })
                .Build();

            await Task.Delay(Timeout.Infinite);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BotSettings>(Configuration.GetSection("BotSettings"));

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
            .AddSingleton<StartupService>()
            .AddSingleton(RabbitHutch.CreateBus(Configuration["rabbitMq:connectionString"]))
            .AddSingleton<MessageService>()
            .AddTransient<NotifyAboutNewEventUseCase>();
        }

        private static void DisplayBanner()
        {
            var name = Assembly.GetCallingAssembly().GetName().Name;
            Console.WriteLine(FiggleFonts.Doom.Render(name!));
        }
    }
}
