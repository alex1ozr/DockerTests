using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.OwnedInstances;
using DockerTestsSample.Api;
using DockerTestsSample.Api.Infrastructure.Filters;
using DockerTestsSample.Api.Infrastructure.Mapping;
using DockerTestsSample.Repositories.Infrastructure.Di;
using DockerTestsSample.ServiceDefaults;
using DockerTestsSample.Services.Infrastructure.Di;
using DockerTestsSample.Store;
using DockerTestsSample.Store.Di;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

builder.AddServiceDefaults();

var serviceName = builder.Environment.ApplicationName;

builder.Configuration.AddEnvironmentVariables();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var config = builder.Configuration;
config.AddEnvironmentVariables("PeopleApi_");

builder.Services
    .AddMvcCore()
    .AddApiExplorer()
    .AddControllersAsServices()
    .AddMvcOptions(opt =>
    {
        opt.Filters.Add<DefaultExceptionFilter>();
    })
    .AddDataAnnotations();

builder.Services.AddFluentValidationAutoValidation(x =>
{
    x.DisableDataAnnotationsValidation = true;
});
builder.Services.AddValidatorsFromAssemblyContaining<IApiMarker>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckl
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "Docker tests sample API";
    settings.Version = "v1";
    settings.UseRouteNameAsOperationId = true;
});
builder.Services.AddAutoMapper(typeof(ApiContractToDtoMappingProfile));
builder.Services.AddPopulationContext("PopulationDb");

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<RepositoriesModule>();
    containerBuilder.RegisterModule<ServicesModule>();
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();
app.MapPrometheusScrapingEndpoint();

var skipMigration = app.Services.GetRequiredService<IConfiguration>()
    .GetSection("SkipMigration").Get<bool?>() ?? false;
if (!skipMigration)
{
    await using var dbContext = app.Services.GetRequiredService<Func<Owned<IPopulationDbContext>>>()();
    await dbContext.Value.Database.MigrateAsync();
}

app.Run();
