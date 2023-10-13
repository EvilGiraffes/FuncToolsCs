namespace FuncToolsCs.Errors;
public sealed class MaybeIsNone : Exception
{
    public MaybeIsNone()
    {
    }

    public MaybeIsNone(string message) : base(message)
    {
    }

    public MaybeIsNone(string message, Exception innerException) : base(message, innerException)
    {
    }
}
