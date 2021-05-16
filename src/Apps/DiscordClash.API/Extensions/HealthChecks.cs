using DiscordClash.API.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordClash.API.Extensions
{
    public static class HealthChecks
    {
        private static string ApplicationName => Assembly.GetEntryAssembly()?.GetName().Name;

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddRabbitMQ(rabbitConnectionString: configuration["rabbitMq:connectionString"])
                .AddMongoDb(configuration["mongoDb:connectionString"]);

            var settings = configuration.GetSection("healthCheckUI").Get<HealthCheckUI>();
            if (settings.IsEnabled)
            {
                services.AddHealthChecksUI(opt =>
                    {
                        opt.SetEvaluationTimeInSeconds(settings.EvaluationTimeInSeconds);
                        opt.AddHealthCheckEndpoint(ApplicationName, "/health");
                    })
                    .AddInMemoryStorage();
            }

            return services;
        }


        public static void UseCustomEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/ping", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self"),
                    ResponseWriter = PongWriteResponse,
                });
                endpoints.MapHealthChecksUI();
                endpoints.MapGet("/", context => context.Response.WriteAsync(ApplicationName ?? "API"));
            });
        }

        private static Task PongWriteResponse(this HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync("pong");
        }
    }
}
