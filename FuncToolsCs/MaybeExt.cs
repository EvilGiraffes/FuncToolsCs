namespace FuncToolsCs;
public static class MaybeExt
{
    public static Maybe<T> ToMaybe<T>(this T? value)
        where T : class
        => Maybe.From(value);
    public static Maybe<(T, TOther)> Zip<T, TOther>(this Maybe<T> maybe, Maybe<TOther> other)
        where T : notnull
        where TOther : notnull
    {
        if (!maybe.TryUnwrap(out T? value) || !other.TryUnwrap(out TOther? otherValue))
            return Maybe<(T, TOther)>.None();
        return Maybe<(T, TOther)>.Some((value, otherValue));
    }
    public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> maybe)
        where T : notnull
        => maybe.Match(
            value => value,
            Maybe.None<T>);
    public static Result<T, TError> ToResult<T, TError>(this Maybe<T> maybe, Func<TError> onNone)
        where T : notnull
        where TError : Exception
        => maybe.Match(
            Result<T, TError>.Ok,
            () => Result<T, TError>.Error(onNone()));
}

