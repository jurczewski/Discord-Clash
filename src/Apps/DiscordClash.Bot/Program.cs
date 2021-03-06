using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordClash.Application.BotHelpers;
using DiscordClash.Application.Endpoints;
using DiscordClash.Application.UseCases.Bot;
using DiscordClash.Bot.Handlers;
using DiscordClash.Infrastructure;
using EasyNetQ;
using Figgle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using System;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordClash.Bot
{
    class Program
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
                    provider.GetRequiredService<LoggingHandler>();
                    provider.GetRequiredService<CommandHandler>();

                    await provider.GetRequiredService<StartupHandler>().StartAsync();

                    var msgService = provider.GetService<MessageHandler>();
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
            .AddSingleton<LoggingHandler>()
            .AddSingleton<CommandHandler>()
            .AddSingleton<StartupHandler>()
            .AddSingleton(RabbitHutch.CreateBus(Configuration["rabbitMq:connectionString"]))
            .AddSingleton<MessageHandler>()
            .AddTransient<NotifyAboutNewEventUseCase>();

            services.AddRefitClient<IDiscordClashApi>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(Configuration["discordClashApi:url"]);
                    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                })
                .AddPolicyHandler(HttpClientInfrastructure.GetNotFoundRetryPolicy())
                .AddPolicyHandler(HttpClientInfrastructure.GetCircuitBreakerPolicy());
        }

        private static void DisplayBanner()
        {
            var name = Assembly.GetCallingAssembly().GetName().Name;
            Console.WriteLine(FiggleFonts.Doom.Render(name!));
        }
    }
}
