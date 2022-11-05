namespace DockerTestsSample.Common.Exceptions;

public abstract class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string message)
        : base(message)
    {
    }
}