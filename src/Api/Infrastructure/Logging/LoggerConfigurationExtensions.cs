using Serilog;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;

namespace DockerTestsSample.Api.Infrastructure.Logging;

internal static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration ConfigureLogger(
        this LoggerConfiguration loggerConfiguration,
        IConfiguration configuration,
        IWebHostEnvironment environment,
        string serviceName)
    {
        return loggerConfiguration
            .ReadFrom.Configuration(configuration)
            //.MinimumLevel.Debug()
            .Enrich.WithProperty("Application", serviceName)
            .Enrich.WithProperty("Environment", environment.EnvironmentName)
            .WriteTo.Logger(lc => lc
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Enrich.With<TraceEnricher>()
                .WriteTo.GrafanaLoki(
                    configuration.GetValue<Uri?>("Loki:Host")?.OriginalString
                    ?? throw new InvalidOperationException("Loki:Host is not configured."),
                    propertiesAsLabels: ["Application", "Environment"])
                .WriteTo.Console());
    }
}