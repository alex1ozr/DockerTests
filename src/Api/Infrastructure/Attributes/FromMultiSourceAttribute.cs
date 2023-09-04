using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DockerTestsSample.Api.Infrastructure.Attributes;

public sealed class FromMultiSourceAttribute : Attribute, IBindingSourceMetadata
{
    public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
        new[] {BindingSource.Path, BindingSource.Query, BindingSource.Body},
        nameof(FromMultiSourceAttribute));
}