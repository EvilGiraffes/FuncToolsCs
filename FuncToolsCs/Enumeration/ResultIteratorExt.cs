using FuncToolsCs.Internal;

namespace FuncToolsCs;
public static class ResultIteratorEx
{
    public static IEnumerable<TItem> Enumerable<TItem, TError>(this Result<IEnumerable<TItem>, TError> result)
        => result.UnwrapOr(() => System.Linq.Enumerable.Empty<TItem>());
    public static Result<IEnumerable<TReturnItem>, TError> Iterate<TItem, TError, TEnumerable, TReturnItem>(
        this Result<TEnumerable, TError> result,
        Func<TItem, Maybe<TReturnItem>> map)
        where TEnumerable : IEnumerable<TItem>
        where TReturnItem : notnull
        => result.Map(enumerable => MaybeEnumeration.Iterate(enumerable, map));
    public static Result<IEnumerable<TReturnItem>, TError> Iterate<TItem, TError, TEnumerable, TReturnItem>(
        this Result<TEnumerable, TError> result,
        Func<TItem, int, Maybe<TReturnItem>> map)
        where TEnumerable : IEnumerable<TItem>
        where TReturnItem : notnull
        => result.Map(enumerable => MaybeEnumeration.Iterate(enumerable, map));
}
