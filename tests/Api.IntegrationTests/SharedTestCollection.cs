using Xunit;

namespace DockerTestsSample.Api.IntegrationTests;

[CollectionDefinition("Tests in Docker collection")]
public class SharedTestCollection : ICollectionFixture<PersonApiFactory>
{
    
}
