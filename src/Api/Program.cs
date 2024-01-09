using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.OwnedInstances;
using DockerTestsSample.Api;
using DockerTestsSample.Api.Infrastructure.Filters;
using DockerTestsSample.Api.Infrastructure.Mapping;
using DockerTestsSample.Repositories.Infrastructure.Di;
using DockerTestsSample.Services.Infrastructure.Di;
using DockerTestsSample.Store;
using DockerTestsSample.Store.Di;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using DockerTestsSample.Api.Infrastructure.Telemetry;
using Serilog;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

builder.Configuration.AddEnvironmentVariables();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
        { Title = "My Sample Service API", Version = "v1" });
});
builder.Services.AddAutoMapper(typeof(ApiContractToDtoMappingProfile));
builder.Services.AddPopulationContext("PopulationDb");

var tracingOtlpEndpoint = builder.Configuration.GetValue<Uri?>("Otlp:Endpoint");
builder.Services
    .AddTelemetry(builder.Environment.ApplicationName, tracingOtlpEndpoint);

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<RepositoriesModule>();
    containerBuilder.RegisterModule<ServicesModule>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

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
