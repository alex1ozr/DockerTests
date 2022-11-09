using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DockerTestsSample.PopulationDbContext.DI;

public sealed class PopulationDbContextModule : Module
{
    public static readonly string ConnectionStringName = "PopulationDbContext";

    protected override void Load(ContainerBuilder containerBuilder)
    {
        var services = new ServiceCollection();
        services.AddDbContextPool<PopulationDbContext>((provider, builder) =>
        {
            var connectionString = provider
                .GetRequiredService<IConfiguration>()
                .GetConnectionString(ConnectionStringName);
            builder.UseNpgsql(connectionString, optionsBuilder => { optionsBuilder.EnableRetryOnFailure(); });
        });
        containerBuilder.Populate(services);

        containerBuilder
            .Register(context => context.Resolve<PopulationDbContext>())
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}