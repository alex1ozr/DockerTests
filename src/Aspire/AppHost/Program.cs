var builder = DistributedApplication.CreateBuilder(args);

var dbName = "PopulationDb";
var postgresDb = builder.AddPostgresContainer(name: "postgres", password: "mysecretpassword")
    .WithAnnotation(new ContainerImageAnnotation { Image = "postgres", Tag = "15.3-alpine" })
    // Set the name of the default database to auto-create on container startup.
    .WithEnvironment("POSTGRES_DB", dbName)
    // Add the default database to the application model so that it can be referenced by other resources.
    .AddDatabase(dbName);

builder.AddProject<Projects.Api>("api")
    .WithReference(postgresDb);

builder.Build().Run();
