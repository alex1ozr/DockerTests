namespace DockerTestsSample.Common.Exceptions;

public abstract class DomainException : Exception
{
    public virtual string ErrorCode => string.Empty;

    public virtual string ShortDescription => string.Empty;

    /// <inheritdoc />
    protected DomainException()
    {
    }

    /// <inheritdoc />
    protected DomainException(string message) : base(message)
    {
    }

    /// <inheritdoc />
    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}