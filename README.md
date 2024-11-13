## About project
Sample of Web API and tests with Database in Docker, along with "build as code" using Nuke Build.

Key features:
- [x] ASP.NET Core 9.0
- [x] Entity Framework Core 9.0 & PostgreSQL 17
- [x] Build solution using [Nuke build](https://nuke.build)
- [x] Dockerfile automatic generation support using [Scriban](https://github.com/scriban/scriban)
- [x] API client generation using [NSwag](https://github.com/RicoSuter/NSwag)
- [x] Integration tests with Docker using [TestContainers](https://github.com/testcontainers/testcontainers-dotnet) and [Respawn](https://github.com/jbogard/Respawn)
- [x] [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) [standalone Dashboard](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/standalone?tabs=bash)
- [x] Docker Compose to run all prerequisites (PostgreSQL, .NET Aspire Dashboard)
- [x] Logging, Metrics, Tracing using [OpenTelemetry](https://opentelemetry.io/)

## Prerequisites

### Main
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)

### Running API
In order to run the API, you need to have a PostgreSQL database running. 
You can use the provided [Docker Compose file](docker-compose.yml) to run it.

List of services:
- PostgreSQL: `localhost:5432`
- Aspire Dashboard: `http://localhost:18888`

## Projects to start
### API
Starts the HTTP-server

### Tests
Execute all tests
```shell
dotnet test
```

Execute only integration tests
```shell
dotnet test --filter Category=IntegrationTests
```

## Create Db Context migrations

```shell
#Install EF utils
dotnet tool install --global dotnet-ef

#Create schema migration
dotnet ef migrations add <Name of your migration> --startup-project src/Api -p src/Store -c PopulationDbContext
```
## Build solution using Nuke build

Install the global tool
```shell
dotnet tool install Nuke.GlobalTool --global
```

### Run build
#### Using a global tool
```shell
nuke
```

#### Using a PowerShell script
Execute from the root of the repository
```shell
pwsh build.ps1 -DockerRepositoriesUrl https://some.docker.registry -Branch some-dev-branch -BuildCounter 1 -DockerRepositoryName some-docker-repo-name -NuGetUrl https://some.nuget.registry -NuGetFeedName some-nuget-feed-name -NuGetApiKey some-nuget-api-key
```

### Execution plan
Execute the following command to see the interactive execution plan
```shell
nuke --plan
```
![Build execution plan](build/BuildExecutionPlan.png)

## API Client
API client is generated using [NSwag](https://github.com/RicoSuter/NSwag) tool.

It generates automatically when ClientGenerator project is built.

See more details in NSwag documentation and [nswag.json](src/ClientGenerator/nswag.json) file.

## Metrics

To see current app metrics, go to http://localhost:5005/metrics