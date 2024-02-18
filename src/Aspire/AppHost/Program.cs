var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Api>("api")
    .WithEnvironment("OTEL_EXPORTER_OTLP_TRACES_ENDPOINT", "http://localhost:16077")
    .WithEnvironment("OTEL_EXPORTER_OTLP_LOGS_ENDPOINT", "http://localhost:16077")
    .WithEnvironment("OTEL_EXPORTER_OTLP_METRICS_ENDPOINT", "http://localhost:16077");

builder.Build().Run();
