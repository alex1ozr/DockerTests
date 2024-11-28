using System.Data.Common;
using DockerTestsSample.Api.IntegrationTests.Infrastructure.Extensions;
using DockerTestsSample.Client.Abstract;
using DockerTestsSample.Client.Extensions;
using DockerTestsSample.Common.Extensions;
using DockerTestsSample.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.Infrastructure;

public sealed class TestApplication :
    WebApplicationFactory<IApiMarker>,
    IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
            .WithImage(DockerImages.PostgreSql)
            .WithDatabase("TestDb")
            .WithUsername("user")
            .WithPassword("password")
            .Build();

    private DbConnection? _dbConnection;
    private Respawner? _respawner;
    private IServiceProvider? _testServices;

    private DbConnection DbConnection => _dbConnection.Required();
    private Respawner Respawner => _respawner.Required();

    public ISampleClient SampleClient => TestServices.GetRequiredService<ISampleClient>();
    
    public IServiceProvider TestServices => _testServices.Required();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseSetting("ConnectionStrings:PopulationDb", _dbContainer.GetConnectionString())
            .UseSetting("Logging:LogLevel:Default", LogLevel.Warning.ToString())
            .UseSetting("Logging:LogLevel:Microsoft", LogLevel.Warning.ToString());
    }

    public async Task ResetDatabaseAsync()
        => await Respawner.ResetAsync(DbConnection);

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await InitializeDatabaseAsync();
        
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawner();
        
        _testServices = ConfigureTestServices();
    }

    private async Task InitializeDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IPopulationDbContext>() as DbContext;
        await context.Required().Database.MigrateAsync();
    }
    
    private async Task InitializeRespawner()
    {
        await DbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(DbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
        });
    }
    
    private IServiceProvider ConfigureTestServices()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();

        var services = new ServiceCollection();

        services.AddTestHosting(Server);
        services.AddSampleClient(options => options.ServerUrl = ClientOptions.BaseAddress);
        services.AddTransient<IConfiguration>(_ => configuration);

        return services.BuildServiceProvider();
    }

    public new async Task DisposeAsync()
        => await _dbContainer.StopAsync();
}
