namespace DockerTestsSample.Common.Exceptions;

public sealed class PersonNotFoundException : DomainException
{
    public Guid Id { get; }

    public PersonNotFoundException(Guid id)
        : base($"The person with id={id} was not found")
    {
        Id = id;
    }

    /// <inheritdoc />
    public override string ErrorCode => "person_not_found";

    /// <inheritdoc />
    public override string ShortDescription => "The person with specified id was not found";
}