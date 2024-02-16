using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace DockerTestsSample.Api.Infrastructure.Logging;

internal sealed class TraceEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current ?? default;
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", activity?.TraceId));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity?.SpanId));
    }
}