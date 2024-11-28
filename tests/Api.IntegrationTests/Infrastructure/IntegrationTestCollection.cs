using Xunit;

namespace DockerTestsSample.Api.IntegrationTests.Infrastructure;

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<TestApplication>
{
    public const string Name = "Integration tests collection";

    public const string Category = "IntegrationTests";
}
