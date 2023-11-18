using Autofac;

namespace DockerTestsSample.Repositories.Infrastructure.Di;

public sealed class RepositoriesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(ThisAssembly)
            .AssignableTo<IDbRepository>()
            .AsImplementedInterfaces()
            .InstancePerDependency();
    }
}