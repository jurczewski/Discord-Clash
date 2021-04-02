using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordClash.API.Extensions
{
    public static class Endpoints
    {
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
                endpoints.MapGet("/", context => context.Response.WriteAsync(Assembly.GetEntryAssembly()?.GetName().Name ?? "API"));
            });
        }

        private static Task PongWriteResponse(this HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync("pong");
        }
    }
}
