using Bogus;
using DockerTestsSample.Api.Contracts.Requests;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.Abstract;

[Trait("Category", IntegrationTestCollection.Category)]
[Collection(IntegrationTestCollection.Name)]
public abstract class ControllerTestsBase: IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    
    protected readonly HttpClient Client;
    protected readonly Faker<PersonRequest> PersonGenerator = new Faker<PersonRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.Name, faker => faker.Person.FirstName)
        .RuleFor(x => x.LastName, faker => faker.Person.LastName)
        .RuleFor(x => x.BirthDate, faker => DateOnly.FromDateTime(faker.Person.DateOfBirth.Date));

    protected ControllerTestsBase(TestApplication apiFactory)
    {
        Client = apiFactory.HttpClient;
        _resetDatabase = apiFactory.ResetDatabaseAsync;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}