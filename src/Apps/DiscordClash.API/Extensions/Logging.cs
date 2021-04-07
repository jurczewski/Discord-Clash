using DiscordClash.API.Framework;
using DiscordClash.API.Framework.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DiscordClash.API.Extensions
{
    public static class Logging
    {
        private const string LoggerSectionName = "logger";

        public static IHostBuilder UseCustomLogging(this IHostBuilder hostBuilder, string applicationName = null, string loggerSectionName = LoggerSectionName)
        {
            var appName = applicationName ?? Assembly.GetExecutingAssembly().FullName;
            return hostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                var loggerOptions = context.Configuration.GetSection(loggerSectionName).Get<LoggerOptions>();

                MapOptions(loggerOptions, loggerConfiguration, context.HostingEnvironment.EnvironmentName, appName);
            });
        }

        private static void MapOptions(LoggerOptions loggerOptions, LoggerConfiguration loggerConfiguration, string environmentName, string appName)
        {
            var level = GetLogEventLevel(loggerOptions.MinimumLevel);

            loggerConfiguration.MinimumLevel.Is(level)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Environment", environmentName)
                    .Enrich.WithProperty("ApplicationName", appName);

            foreach (var (key, value) in loggerOptions.Tags ?? new Dictionary<string, object>())
            {
                loggerConfiguration.Enrich.WithProperty(key, value);
            }

            foreach (var (key, value) in loggerOptions.MinimumLevelOverrides ?? new Dictionary<string, string>())
            {
                var logLevel = GetLogEventLevel(value);
                loggerConfiguration.MinimumLevel.Override(key, logLevel);
            }

            loggerOptions.ExcludePaths?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.EndsWith(p))));

            loggerOptions.ExcludeProperties?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty(p)));

            Configure(loggerConfiguration, level, loggerOptions);
        }

        private static void Configure(LoggerConfiguration loggerConfiguration, LogEventLevel level, LoggerOptions options)
        {
            var consoleOptions = options.Console ?? new ConsoleOptions();
            var fileOptions = options.File ?? new FileOptions();
            var dataDogOptions = options.DataDog ?? new DataDogOptions();

            if (consoleOptions.Enabled)
            {
                var format = consoleOptions.Format;
                if (string.Equals(format, "compact", StringComparison.OrdinalIgnoreCase))
                {
                    loggerConfiguration.WriteTo.Console(new CompactJsonFormatter());
                }
                else if (string.Equals(format, "colored", StringComparison.OrdinalIgnoreCase))
                {
                    loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Code);
                }
                else
                {
                    loggerConfiguration.WriteTo.Console();
                }
            }

            if (fileOptions.Enabled)
            {
                var path = string.IsNullOrWhiteSpace(fileOptions.Path) ? "logs/logs.txt" : fileOptions.Path;
                if (!Enum.TryParse<RollingInterval>(fileOptions.Interval, true, out var interval))
                {
                    interval = RollingInterval.Day;
                }

                loggerConfiguration.WriteTo.File(path, rollingInterval: interval);
            }

            if (dataDogOptions.Enabled)
            {
                loggerConfiguration.WriteTo.DatadogLogs(dataDogOptions.ApiKey, dataDogOptions.Source, dataDogOptions.Service, tags: dataDogOptions.Tags, logLevel: level);
            }
        }

        private static LogEventLevel GetLogEventLevel(string level)
            => Enum.TryParse<LogEventLevel>(level, true, out var logLevel)
                ? logLevel
                : LogEventLevel.Information;
    }
}
