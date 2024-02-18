using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.OpenTelemetry;

namespace DockerTestsSample.ServiceDefaults.Logging;

public static class SerilogExtensions
{
    /// <summary>
    /// Configure Serilog to write to the console and OpenTelemetry for Aspire structured logs.
    /// </summary>
    /// <remarks>
    /// ⚠ This method MUST be called before the <see cref="OpenTelemetryLoggingExtensions.AddOpenTelemetry(ILoggingBuilder)"/> method to still send structured logs via OpenTelemetry. ⚠
    /// </remarks>
    internal static IHostApplicationBuilder ConfigureSerilog(this IHostApplicationBuilder builder, string serviceName)
    {
        var environment = builder.Environment;
        Log.Logger.Information("App Service name {Name}", serviceName);
        
        builder.Services.AddSerilog((_, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                //.MinimumLevel.Debug()
                .Enrich.WithProperty("Application", serviceName)
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .Enrich.With<TraceEnricher>()
                    //.ConditionalWriteToGrafanaLoki(builder.Configuration)
                    .ConditionalWriteToOtlp(builder.Configuration, serviceName)
                    .WriteTo.Console());
        });

        // Removes the built-in logging providers
        builder.Logging.ClearProviders().AddSerilog();
        return builder;
    }
    
    private static LoggerConfiguration ConditionalWriteToGrafanaLoki(
        this LoggerConfiguration loggerConfiguration, 
        IConfiguration configuration)
    {
        var lokiExporter = configuration["Loki:Host"];
        if (!string.IsNullOrEmpty(lokiExporter))
        {
            loggerConfiguration.WriteTo.GrafanaLoki(lokiExporter,
                propertiesAsLabels: ["Application", "Environment"]);
        }

        return loggerConfiguration;
    }
    
    private static LoggerConfiguration ConditionalWriteToOtlp(
        this LoggerConfiguration loggerConfiguration, 
        IConfiguration configuration,
        string serviceName)
    {
        var otlpExporter = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"];
        if (!string.IsNullOrEmpty(otlpExporter))
        {
            loggerConfiguration
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = otlpExporter;
                    options.Protocol = OtlpProtocol.HttpProtobuf;
                    options.ResourceAttributes.Add("service.name", serviceName);
                });
        }

        return loggerConfiguration;
    }

    /// <summary>
    /// Sets up a initial logger to catch and report exceptions thrown during set-up of the ASP.NET Core host.
    /// This follows the Two-stage initialization process documented <a href="https://github.com/serilog/serilog-aspnetcore?tab=readme-ov-file#two-stage-initialization">here</a>.
    /// </summary>
    /// <param name="logger">Only used for the extension method base type</param>
    public static Serilog.ILogger ConfigureSerilogBootstrapLogger(this Serilog.ILogger logger)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        return logger;
    }
}