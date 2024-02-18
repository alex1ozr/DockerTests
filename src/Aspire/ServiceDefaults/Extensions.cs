using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DockerTestsSample.ServiceDefaults;

public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        var serviceName = builder.Configuration["OTEL_SERVICE_NAME"] ?? "service";
            
        builder.ConfigureOpenTelemetry(serviceName);

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.UseServiceDiscovery();
        });

        return builder;
    }

    public static IHostApplicationBuilder ConfigureOpenTelemetry(
        this IHostApplicationBuilder builder,
        string serviceName)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddRuntimeInstrumentation()
                    .AddBuiltInMeters()
                    .AddProcessInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddPrometheusExporter();
            })
            .WithTracing(tracing =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    // We want to view all traces in development
                    tracing.SetSampler(new AlwaysOnSampler());
                }

                tracing.AddAspNetCoreInstrumentation(options =>
                    {
                        options.Filter += context =>
                            context.Request.Path.Value?.Contains("metrics",
                                StringComparison.InvariantCultureIgnoreCase) == false
                            && context.Request.Path.Value?.Contains("swagger",
                                StringComparison.InvariantCultureIgnoreCase) == false;

                        options.EnrichWithHttpResponse = (activity, response) =>
                            activity.AddTag("error", response.StatusCode >= 400);
                    })
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true);
            });
            
        builder.AddOpenTelemetryExporters();
            
        return builder;
    }
        
    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var otlpExporterTracesEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_TRACES_ENDPOINT"];
        if (!string.IsNullOrWhiteSpace(otlpExporterTracesEndpoint))
        {
            Console.WriteLine("Using OTLP Traces exporter");
            builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(otlpExporterTracesEndpoint);
            }));
        }
        else
        {
            builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddConsoleExporter());
        }
            
        var otlpExporterLogsEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"];
        if (!string.IsNullOrWhiteSpace(otlpExporterLogsEndpoint))
        {
            builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter(
                options =>
                {
                    options.Endpoint = new Uri(otlpExporterLogsEndpoint);

                    if (builder.Configuration["OTEL_EXPORTER_OTLP_LOGS_PROTOCOL"] == "http/protobuf")
                    {
                        options.Protocol = OtlpExportProtocol.HttpProtobuf;
                    }
                }));
        }

        var otlpExporterMetricsEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_METRICS_ENDPOINT"];
        if (!string.IsNullOrWhiteSpace(otlpExporterMetricsEndpoint))
        {
            builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter(
                options =>
                {
                    options.Endpoint = new Uri(otlpExporterMetricsEndpoint);
                }));
        }

        // Uncomment the following lines to enable the Prometheus exporter (requires the OpenTelemetry.Exporter.Prometheus.AspNetCore package)
        //builder.Services.AddOpenTelemetry()
        //   .WithMetrics(metrics => metrics.AddPrometheusExporter());

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
        // builder.Services.AddOpenTelemetry()
        //    .UseAzureMonitor();

        return builder;
    }

    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Uncomment the following line to enable the Prometheus endpoint (requires the OpenTelemetry.Exporter.Prometheus.AspNetCore package)
        // app.MapPrometheusScrapingEndpoint();

        // All health checks must pass for app to be considered ready to accept traffic after starting
        app.MapHealthChecks("/health");

        // Only health checks tagged with the "live" tag must pass for app to be considered alive
        app.MapHealthChecks("/alive", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });

        return app;
    }

    private static MeterProviderBuilder AddBuiltInMeters(this MeterProviderBuilder meterProviderBuilder) =>
        meterProviderBuilder.AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "System.Net.Http");
}