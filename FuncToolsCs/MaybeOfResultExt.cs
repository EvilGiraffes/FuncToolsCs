namespace FuncToolsCs;
public static class MaybeOfResultExt
{
    public static TReturn Match<T, TReturn, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, TReturn> onOk,
        Func<TError, TReturn> onError,
        TReturn onNone)
        => maybe.Match(
            result => result.Match(onOk, onError),
            onNone);
    public static TReturn Match<T, TReturn, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, TReturn> onOk,
        Func<TError, TReturn> onError,
        Func<TReturn> onNone)
        => maybe.Match(
            result => result.Match(onOk, onError),
            onNone);
    public static Result<Maybe<T>, TError> Transpose<T, TError>(this Maybe<Result<T, TError>> maybe)
        where T : notnull
        => Match(maybe,
            value => Result<Maybe<T>, TError>.Ok(Maybe.Some(value)),
            Result<Maybe<T>, TError>.Error,
            () => Result<Maybe<T>, TError>.Ok(Maybe.None<T>()));
    public static bool NestedIsError<T, TError>(this Maybe<Result<T, TError>> maybe)
        => maybe.Match(
            result => result.IsError,
            false);
    public static bool NestedIsOk<T, TError>(this Maybe<Result<T, TError>> maybe)
        => maybe.Match(
            result => result.IsOk,
            false);
    public static Maybe<Result<TReturn, TError>> NestedMap<T, TReturn, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, TReturn> map)
        => maybe.Map(result => result.Map(map));
    public static Maybe<Result<T, TReturnError>> NestedMapError<T, TError, TReturnError>(
        this Maybe<Result<T, TError>> maybe,
        Func<TError, TReturnError> map)
        => maybe.Map(result => result.MapError(map));
    public static Maybe<Result<TReturn, TError>> NestedBind<T, TReturn, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, Result<TReturn, TError>> binder)
        => maybe.Map(result => result.Bind(binder));
    public static Maybe<Result<TReturn, TReturnError>> NestedBindOrMapError<T, TReturn, TError, TReturnError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, Result<TReturn, TReturnError>> binder,
        Func<TError, TReturnError> onError)
        => maybe.Map(result => result.BindOrMapError(binder, onError));
    public static Maybe<Result<T, TError>> NestedHandle<T, TError>(this Maybe<Result<T, TError>> maybe, Action<TError> onError)
        => maybe.Map(result => result.Handle(onError));
    public static Maybe<Result<T, TError>> NestedInspect<T, TError>(this Maybe<Result<T, TError>> maybe, Action<T> onOk)
        => maybe.Map(result => result.Inspect(onOk));
    public static Maybe<TReturn> NestedMatch<T, TReturn, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T, TReturn> onOk,
        Func<TError, TReturn> onError)
        where TReturn : notnull
        => maybe.Map(result => result.Match(onOk, onError));
    public static Maybe<T> NestedUnwrap<T, TError>(this Maybe<Result<T, TError>> maybe)
        where T : notnull
        => maybe.Map(result => result.Unwrap());
    public static Maybe<T> NestedUnwrapOrThrow<T, TError, TThrownError>(
        this Maybe<Result<T, TError>> maybe,
        Func<TError, TThrownError> onError)
        where T : notnull
        where TThrownError : Exception
        => maybe.Map(result => result.UnwrapOrThrow(onError));
    public static Maybe<T> NestedUnwrapOr<T, TError>(
        this Maybe<Result<T, TError>> maybe,
        T defaultValue)
        where T : notnull
        => maybe.Map(result => result.UnwrapOr(defaultValue));
    public static Maybe<T> NestedUnwrapOr<T, TError>(
        this Maybe<Result<T, TError>> maybe,
        Func<T> defaultValueFactory)
        where T : notnull
        => maybe.Map(result => result.UnwrapOr(defaultValueFactory));
}
