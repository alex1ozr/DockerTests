using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;

namespace DockerTestsSample.Api.Infrastructure.Logging;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogging(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment,
        string serviceName)
    {
        return services.AddLogging(b =>
            b.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Application", serviceName)
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .Enrich.With<TraceEnricher>()
                    .WriteTo.GrafanaLoki(
                        configuration.GetValue<Uri?>("Loki:Host")?.OriginalString
                        ?? throw new InvalidOperationException("Loki:Host is not configured."),
                        propertiesAsLabels: ["Application", "Environment"]))
                .CreateLogger()));
    }
}

internal sealed class TraceEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current ?? default;
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", activity?.TraceId));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity?.SpanId));
    }
}