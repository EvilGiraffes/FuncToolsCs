namespace FuncToolsCs.Errors;
public class ResultIsError<TError> : Exception
{
    public TError Error { get; init; } = default!;
    public override string Message
        => $"{base.Message}{Environment.NewLine}Error is defined as : {Error}";
    public ResultIsError()
    {
    }

    public ResultIsError(string message) : base(message)
    {
    }

    public ResultIsError(string message, Exception innerException) : base(message, innerException)
    {
    }
}
