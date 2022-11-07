using Autofac;
using DockerTestsSample.Repositories.Abstract;

namespace DockerTestsSample.Repositories.Infrastructure.DI;

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