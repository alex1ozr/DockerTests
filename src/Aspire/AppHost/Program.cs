using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var dbName = "PopulationDb";
var postgresDb = builder
    .AddPostgresContainer(name: "PopulationDb", port: 5432, password: "mysecretpassword")
    .WithAnnotation(new ContainerImageAnnotation { Image = "postgres", Tag = "15.3-alpine" })
    // Set the name of the default database to auto-create on container startup.
    .WithEnvironment("POSTGRES_DB", dbName);

if (builder.Environment.IsDevelopment())
{
    postgresDb
        // Mount the Postgres data directory into the container so that the database is persisted
        .WithVolumeMount("../../../data/postgres-mnt", "/var/lib/postgresql/data", VolumeMountType.Bind);
}

builder.AddProject<Projects.Api>("api")
    .WithReference(postgresDb);

builder.Build().Run();
