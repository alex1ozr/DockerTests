using Xunit;

namespace DockerTestsSample.Api.IntegrationTests;

[CollectionDefinition("Integration tests collection")]
public class IntegrationTestCollection : ICollectionFixture<TestApplication>
{
    
}
