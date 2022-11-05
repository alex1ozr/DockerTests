using Autofac;
using DockerTestsSample.Repositories.Infrastructure.DI;
using DockerTestsSample.Services.Abstract;

namespace DockerTestsSample.Services.Infrastructure.DI;

public sealed class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<RepositoriesModule>();
        
        builder
            .RegisterAssemblyTypes(typeof(RepositoriesModule).Assembly)
            .AssignableTo<IBusinessService>()
            .AsImplementedInterfaces()
            .InstancePerDependency();
    }
}