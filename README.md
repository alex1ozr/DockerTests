## About project
Sample of Web API and tests with Database in Docker (using [TestContainers](https://github.com/testcontainers/testcontainers-dotnet) & [Respawn](https://github.com/jbogard/Respawn) libraries)

## Projects to start
### Api
Starts the HTTP-server

### Tests
```shell
dotnet test
```

## Create Db Context migrations

```shell
#Install EF utils
dotnet tool install --global dotnet-ef

#Create schema migration
dotnet ef migrations add InitialMigration --startup-project src/Api -p src/PopulationDbContext -c PopulationDbContext
```
