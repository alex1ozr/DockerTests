using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DockerTestsSample.Api.Infrastructure.Telemetry;

internal static class ServiceCollectionExtensions
{
    public static void AddTelemetry(
        this IServiceCollection services,
        string serviceName,
        Uri? tracingOtlpEndpoint = null,
        Uri? jaegerEndpoint = null)
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
                tracing.AddAspNetCoreInstrumentation(options =>
                {
                    options.Filter += context =>
                        context.Request.Path.Value?.Contains("metrics", StringComparison.InvariantCultureIgnoreCase) == false
                        && context.Request.Path.Value?.Contains("swagger", StringComparison.InvariantCultureIgnoreCase) == false;
                    
                    options.EnrichWithHttpResponse = (activity, response) =>
                        activity.AddTag("error", response.StatusCode >= 400);
                });
                tracing.AddHttpClientInstrumentation();
                tracing.AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true);

                if (tracingOtlpEndpoint != null)
                {
                    tracing.AddOtlpExporter(options => options.Endpoint = tracingOtlpEndpoint);
                }
                else if (jaegerEndpoint != null)
                {
                    tracing.AddJaegerExporter(options => options.Endpoint = jaegerEndpoint);
                }
                else
                {
                    tracing.AddConsoleExporter();
                }
            });
    }
}