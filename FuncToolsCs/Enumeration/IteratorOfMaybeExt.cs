namespace FuncToolsCs.Enumeration;
public static class IteratorOfMaybeExt
{
    public static IEnumerable<T> Extract<T>(this IEnumerable<Maybe<T>> enumerable)
        where T : notnull
    {
        foreach (Maybe<T> maybe in enumerable)
        {
            if (!maybe.TryUnwrap(out T? value))
                continue;
            yield return value;
        }
    }
    public static IEnumerable<T> ExtractIf<T>(this IEnumerable<Maybe<T>> enumerable, Func<T, bool> predicate)
        where T : notnull
    {
        foreach (T value in enumerable.Extract())
        {
            if (!predicate(value))
                continue;
            yield return value;
        }
    }
}
