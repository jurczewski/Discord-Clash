using DiscordClash.Application.Endpoints;
using DiscordClash.Application.UseCases.Recommender;
using DiscordClash.Infrastructure;
using Figgle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using Serilog;
using System;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordClash.Recommender
{
    class Program
    {
        private static IConfigurationRoot Configuration { get; set; }

        private static async Task Main(string[] args)
        {
            DisplayBanner();

            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("appsettings.json", true, true)
                        .AddEnvironmentVariables();
                    Configuration = builder.Build();
                })
                .UseCustomLogging()
                .ConfigureServices(RegisterServices)
                .Build();

            await ParseArgs(host, args[0]);
        }

        private static async Task ParseArgs(IHost host, string command)
        {
            switch (command)
            {
                case "train":
                    var svc = ActivatorUtilities.CreateInstance<TrainModelAndSaveItUseCase>(host.Services);
                    await svc.Execute();
                    break;
                default:
                    Log.Logger.Information("Invalid command");
                    break;
            }
        }

        private static void RegisterServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddTransient<TrainModelAndSaveItUseCase>();

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
