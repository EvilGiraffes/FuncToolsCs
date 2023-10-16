namespace FuncToolsCs;
public static class MaybeOfResultExt
{
    public static TReturn Match<T, TReturn, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, TReturn> onOk,
        Func<TError, TReturn> onError,
        TReturn onNone)
        where TError : Exception
        => maybe.Match(
            result => result.Match(onOk, onError),
            onNone);
    public static TReturn Match<T, TReturn, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, TReturn> onOk,
        Func<TError, TReturn> onError,
        Func<TReturn> onNone)
        where TError : Exception
        => maybe.Match(
            result => result.Match(onOk, onError),
            onNone);
    public static Result<Maybe<T>, TError> Transpose<T, TError>(this Maybe<Result<T, TError>> maybe)
        where T : notnull
        where TError : Exception
        => Match(maybe,
            value => Result<Maybe<T>, TError>.Ok(Maybe.Some(value)),
            Result<Maybe<T>, TError>.Error,
            () => Result<Maybe<T>, TError>.Ok(Maybe.None<T>()));
    public static bool NestedIsError<T, TError>(this Maybe<Result<T, TError>> maybe)
        where TError : Exception
        => maybe.Match(
            result => result.IsError,
            false);
    public static bool NestedIsOk<T, TError>(this Maybe<Result<T, TError>> maybe)
        where TError : Exception
        => maybe.Match(
            result => result.IsOk,
            false);
    public static Maybe<Result<TReturn, TError>> NestedMap<T, TReturn, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, TReturn> map)
        where TError : Exception
        => maybe.Map(result => result.Map(map));
    public static Maybe<Result<T, TReturnError>> NestedMapError<T, TError, TReturnError>(
        this Maybe<Result<T, TError>> maybe,
        Func<TError, TReturnError> map)
        where TError : Exception
        where TReturnError : Exception
        => maybe.Map(result => result.MapError(map));
    public static Maybe<Result<TReturn, TError>> NestedBind<T, TReturn, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, Result<TReturn, TError>> binder)
        where TError : Exception
        => maybe.Map(result => result.Bind(binder));
    public static Maybe<Result<TReturn, TReturnError>> NestedBindOrMapError<T, TReturn, TError, TReturnError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, Result<TReturn, TReturnError>> binder,
        Func<TError, TReturnError> onError)
        where TError : Exception
        where TReturnError : Exception
        => maybe.Map(result => result.BindOrMapError(binder, onError));
    public static Maybe<TReturn> NestedMatch<T, TReturn, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, TReturn> onOk,
        Func<TError, TReturn> onError)
        where TReturn : notnull
        where TError : Exception
        => maybe.Map(result => result.Match(onOk, onError));
    public static Maybe<T> NestedUnwrap<T, TError>(this Maybe<Result<T, TError>> maybe)
        where T : notnull
        where TError : Exception
        => maybe.Map(result => result.Unwrap());
    public static Maybe<T> NestedUnwrapOrThrow<T, TError, TMappedError>(
        this Maybe<Result<T, TError>> maybe,
        Func<TError, TMappedError> onError)
        where T : notnull
        where TError : Exception
        where TMappedError : Exception
        => maybe.Map(result => result.UnwrapOrThrow(onError));
    public static Maybe<T> NestedUnwrapOr<T, TError>(
        this Maybe<Result<T, TError>> maybe,
        T defaultValue)
        where T : notnull
        where TError : Exception
        => maybe.Map(result => result.UnwrapOr(defaultValue));
    public static Maybe<T> NestedUnwrapOr<T, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T> defaultValueFactory)
        where T : notnull
        where TError : Exception
        => maybe.Map(result => result.UnwrapOr(defaultValueFactory));
}
