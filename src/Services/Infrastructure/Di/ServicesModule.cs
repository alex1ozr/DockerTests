using Autofac;
using Autofac.Extensions.DependencyInjection;
using DockerTestsSample.Services.Infrastructure.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DockerTestsSample.Services.Infrastructure.Di;

public sealed class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddAutoMapper(typeof(DtoToEntitiesMappingProfile));
        builder.Populate(serviceCollection);

        builder
            .RegisterAssemblyTypes(ThisAssembly)
            .AssignableTo<IBusinessService>()
            .AsImplementedInterfaces()
            .InstancePerDependency();
    }
}