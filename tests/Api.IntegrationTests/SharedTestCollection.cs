using Xunit;

namespace DockerTestsSample.Api.IntegrationTests;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<PersonApiFactory>
{
    
}
