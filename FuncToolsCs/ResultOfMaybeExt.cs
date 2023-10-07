namespace FuncToolsCs;
public static class ResultOfMaybeExt
{
    public static Maybe<Result<T, TError>> Transpose<T, TError>(this Result<Maybe<T>, TError> result)
        where T : notnull
        where TError : Exception
        => Match(result,
            value => Maybe.Some(Result<T, TError>.Ok(value)),
            Maybe.None<Result<T, TError>>,
            error => Maybe.Some(Result<T, TError>.Error(error)));
    public static TReturn Match<T, TReturn, TError>(this Result<Maybe<T>, TError> result, Func<T, TReturn> onSome, TReturn onNone, Func<TError, TReturn> onError)
        where T : notnull
        where TError : Exception
        => result.Match(
            maybe => maybe.Match(onSome, onNone),
            onError);
    public static TReturn Match<T, TReturn, TError>(this Result<Maybe<T>, TError> result, Func<T, TReturn> onSome, Func<TReturn> onNone, Func<TError, TReturn> onError)
        where T : notnull
        where TError : Exception
        => result.Match(
            maybe => maybe.Match(onSome, onNone),
            onError);
    public static bool NestedIsNone<T, TError>(this Result<Maybe<T>, TError> result)
        where T : notnull
        where TError : Exception
        => result.Match(
            maybe => maybe.IsNone,
            _ => false);
    public static bool NestedIsSome<T, TError>(this Result<Maybe<T>, TError> result)
        where T : notnull
        where TError : Exception
        => result.Match(
            maybe => maybe.IsSome,
            _ => false);
    public static Result<Maybe<TReturn>, TError> NestedMap<T, TReturn, TError>(this Result<Maybe<T>, TError> result, Func<T, TReturn> map)
        where T : notnull
        where TReturn : notnull
        where TError : Exception
        => result.Map(maybe => maybe.Map(map));
    public static Result<Maybe<TReturn>, TError> NestedBind<T, TReturn, TError>(this Result<Maybe<T>, TError> result, Func<T, Maybe<TReturn>> bind)
        where T : notnull
        where TReturn : notnull
        where TError : Exception
        => result.Map(maybe => maybe.Bind(bind));
    public static Result<Maybe<T>, TError> NestedFilter<T, TError>(this Result<Maybe<T>, TError> result, Predicate<T> filter)
        where T : notnull
        where TError : Exception
        => result.Map(maybe => maybe.Filter(filter));
    public static Result<TReturn, TError> NestedMatch<T, TReturn, TError>(this Result<Maybe<T>, TError> result, Func<T, TReturn> onSome, TReturn onNone)
        where T : notnull
        where TError : Exception
        => result.Map(maybe => maybe.Match(onSome, onNone));
    public static Result<TReturn, TError> NestedMatch<T, TReturn, TError>(this Result<Maybe<T>, TError> result, Func<T, TReturn> onSome, Func<TReturn> onNone)
        where T : notnull
        where TError : Exception
        => result.Map(maybe => maybe.Match(onSome, onNone));
    public static Result<T?, TError> NestedUnwrap<T, TError>(this Result<Maybe<T>, TError> result)
        where T : notnull
        where TError : Exception
        => result.Map(maybe => maybe.Unwrap());
    public static Result<T, TReturnError> NestedUnwrapResult<T, TError, TReturnError>(this Result<Maybe<T>, TError> result, Func<Maybe<TError>, TReturnError> errorMap)
        where T : notnull
        where TError : Exception
        where TReturnError : Exception
        => result.BindOrMapError(
            maybe => maybe.UnwrapResult(
                () => errorMap(
                    Maybe.None<TError>())),
            error => errorMap(
                Maybe.Some(error)));
    public static Result<T, TError> NestedUnwrapOrThrow<T, TError, TThrownError>(this Result<Maybe<T>, TError> result, Func<TThrownError> exceptionFactory)
        where T : notnull
        where TError : Exception
        where TThrownError : Exception
        => result.Map(maybe => maybe.UnwrapOrThrow(exceptionFactory));
    public static Result<T, TError> NestedUnwrapOr<T, TError>(this Result<Maybe<T>, TError> result, T defaultValue)
        where T : notnull
        where TError : Exception
        => result.Map(maybe => maybe.UnwrapOr(defaultValue));
    public static Result<T, TError> NestedUnwrapOr<T, TError>(this Result<Maybe<T>, TError> result, Func<T> defaultValue)
        where T : notnull
        where TError : Exception
        => result.Map(maybe => maybe.UnwrapOr(defaultValue));
}
