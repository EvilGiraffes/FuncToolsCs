namespace FuncToolsCs.Errors;
public sealed class UnwrappingFailed : Exception
{
    public UnwrappingFailed()
    {
    }

    public UnwrappingFailed(string message) : base(message)
    {
    }

    public UnwrappingFailed(string message, Exception innerException) : base(message, innerException)
    {
    }
}
