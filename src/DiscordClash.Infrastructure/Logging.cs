using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;

namespace DiscordClash.Infrastructure
{
    public static class Logging
    {
        public static IHostBuilder UseCustomLogging(this IHostBuilder hostBuilder, string applicationName = null)
        {
            var appName = applicationName ?? Assembly.GetExecutingAssembly().FullName;

            return hostBuilder.UseSerilog(new LoggerConfiguration()
                .MinimumLevel.Is(LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", appName)
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger()
            );
        }
    }
}
