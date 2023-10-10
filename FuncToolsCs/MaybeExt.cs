using System.Diagnostics.CodeAnalysis;

namespace FuncToolsCs;
public static class MaybeExt
{
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
    public static bool TryGetSome<T>(this Maybe<T> maybe, [NotNullWhen(true)] out T? result)
        where T : notnull
    {
        if (maybe.IsNone)
        {
            result = default;
            return false;
        }
        result = maybe.UnwrapGuaranteed();
        return true;
    }
}

