using System.Data.Common;
using DockerTestsSample.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests;

public sealed class TestApplication : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = 
        new PostgreSqlBuilder()
        .WithDatabase("TestDb")
        .WithUsername("user")
        .WithPassword("password").Build();
    
    private DbConnection? _dbConnection;
    private Respawner? _respawner;
    private HttpClient? _httpClient;

    private DbConnection DbConnection => _dbConnection.Required();
    private Respawner Respawner => _respawner.Required();
    public HttpClient HttpClient => _httpClient.Required();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder) 
        => builder.UseSetting("ConnectionStrings:PopulationDbContext", _dbContainer.GetConnectionString());

    public async Task ResetDatabaseAsync() 
        => await Respawner.ResetAsync(DbConnection);

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        _httpClient = CreateClient();
        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await DbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(DbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" }
        });
    }

    public new async Task DisposeAsync() 
        => await _dbContainer.StopAsync();
}
