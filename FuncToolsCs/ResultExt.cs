namespace FuncToolsCs;
public static class ResultExt
{
    public static Result<T, TInnerError> Flatten<T, TError, TInnerError>(this Result<Result<T, TInnerError>, TError> result, Func<TError, TInnerError> onError)
        where TError : Exception
        where TInnerError : Exception
        => result.Match(
            inner => inner,
            error => onError(error));
    public static Maybe<T> ToMaybe<T, TError>(this Result<T, TError> result)
        where T : notnull
        where TError : Exception
        => result.Match(
            value => Maybe.Some(value),
            _ => Maybe.None<T>());
}
