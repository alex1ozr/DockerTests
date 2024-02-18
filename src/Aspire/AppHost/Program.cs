var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Api>("api");

builder.Build().Run();
