using Bogus;
using DockerTestsSample.Api.Contracts.Requests;
using DockerTestsSample.Common.Extensions;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.Abstract;

public abstract class ControllerTestsBase: IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    
    protected readonly HttpClient _client;
    protected readonly Faker<PersonRequest> PersonGenerator = new Faker<PersonRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.Name, faker => faker.Person.FirstName)
        .RuleFor(x => x.LastName, faker => faker.Person.LastName)
        .RuleFor(x => x.BirthDate, faker => faker.Person.DateOfBirth.Date.SetKindUtc());

    public ControllerTestsBase(PersonApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;
        _resetDatabase = apiFactory.ResetDatabaseAsync;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}