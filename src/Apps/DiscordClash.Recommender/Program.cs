using Cocona;
using Cocona.Hosting;
using DiscordClash.Application.Endpoints;
using DiscordClash.Application.Queries;
using DiscordClash.Application.UseCases.Recommender;
using DiscordClash.Infrastructure;
using Figgle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
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

            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("appsettings.json", true, true)
                        .AddEnvironmentVariables();
                    Configuration = builder.Build();
                })
                .UseCustomLogging()
                .ConfigureServices(RegisterServices);

            var coconaHostBuilder = new CoconaAppHostBuilder(hostBuilder);
            await coconaHostBuilder.RunAsync<Program>(args);
        }

        [PrimaryCommand]
        [Command("train", Description = "Trains model and saves it.")]
        public async Task TrainModelAndSaveIt([FromService] TrainModelAndSaveItUseCase consoleUseCase)
        {
            await consoleUseCase.Execute();
        }

        [Command("rec", Description = "Loads model and use it.")]
        public void LoadAndRecommend([FromService] LoadAndRecommendUseCase consoleUseCase)
        {
            var choice = new ChoiceDto { UserId = Guid.Parse("eacf1405-e4cc-41aa-b65e-8752dfaad5de"), EventId = Guid.Parse("43addb42-4bdc-48fb-a04d-7857ffcc8f2a") };
            consoleUseCase.Execute(choice);
        }

        private static void RegisterServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddTransient<TrainModelAndSaveItUseCase>();
            services.AddTransient<LoadAndRecommendUseCase>();

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
