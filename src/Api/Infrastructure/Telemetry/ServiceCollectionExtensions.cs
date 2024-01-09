using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DockerTestsSample.Api.Infrastructure.Telemetry;

internal static class ServiceCollectionExtensions
{
    public static void AddTelemetry(
        this IServiceCollection services,
        string serviceName,
        Uri? tracingOtlpEndpoint = null)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithMetrics(metrics => metrics
                .AddRuntimeInstrumentation()
                .AddProcessInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddPrometheusExporter())
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                if (tracingOtlpEndpoint != null)
                {
                    tracing.AddOtlpExporter(otlpOptions => otlpOptions.Endpoint = tracingOtlpEndpoint);
                }
                else
                {
                    tracing.AddConsoleExporter();
                }
            });
    }
}