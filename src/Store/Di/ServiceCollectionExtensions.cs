using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DockerTestsSample.Store.Di;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register the Db Context
    /// </summary>
    public static IServiceCollection AddPopulationContext(
        this IServiceCollection services,
        string connectionStringName)
    {
        services.AddDbContext<PopulationDbContext>((provider, builder) =>
        {
            var connectionString = provider.GetRequiredService<IConfiguration>()
                                       .GetConnectionString(connectionStringName)
                                   ?? throw new InvalidOperationException($"Connection string {connectionStringName} is not found");
            builder.UseNpgsql(connectionString, optionsBuilder =>
            {
                optionsBuilder.EnableRetryOnFailure();
            });
        });

        services.AddScoped<IPopulationDbContext>(provider => provider.GetRequiredService<PopulationDbContext>());

        return services;
    }
}
