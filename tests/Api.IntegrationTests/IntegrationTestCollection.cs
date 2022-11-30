using Xunit;

namespace DockerTestsSample.Api.IntegrationTests;

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<TestApplication>
{
    public const string Name = "Integration tests collection";
}
