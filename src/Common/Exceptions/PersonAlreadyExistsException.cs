namespace DockerTestsSample.Common.Exceptions;

public sealed class PersonAlreadyExistsException : DomainException
{
    public Guid Id { get; }

    public PersonAlreadyExistsException(Guid id)
        : base($"The person with id={id} already exists")
    {
        Id = id;
    }

    /// <inheritdoc />
    public override string ErrorCode => "person_already_exists";

    /// <inheritdoc />
    public override string ShortDescription => "The person with specified id already exists";
}