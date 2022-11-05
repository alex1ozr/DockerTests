namespace DockerTestsSample.Common.Exceptions;

public sealed class PersonAlreadyExistsException : DomainException
{
    public Guid Id { get; }

    public PersonAlreadyExistsException(Guid id)
        : base($"The person with id={id} already exists")
    {
        Id = id;
    }
}