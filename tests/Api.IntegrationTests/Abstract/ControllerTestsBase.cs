using Bogus;
using DockerTestsSample.Client.Abstract;
using DockerTestsSample.Client.Implementations;
using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.Abstract;

[Trait("Category", IntegrationTestCollection.Category)]
[Collection(IntegrationTestCollection.Name)]
public abstract class ControllerTestsBase: IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    
    protected readonly HttpClient HttpClient;
    protected readonly ISampleClient Client;
    
    protected readonly Faker<PersonRequest> PersonGenerator = new Faker<PersonRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.Name, faker => faker.Person.FirstName)
        .RuleFor(x => x.LastName, faker => faker.Person.LastName)
        .RuleFor(x => x.BirthDate, faker => faker.Person.DateOfBirth.Date);

    protected ControllerTestsBase(TestApplication testApplication)
    {
        HttpClient = testApplication.HttpClient;
        Client = testApplication.SampleClient;
        _resetDatabase = testApplication.ResetDatabaseAsync;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}