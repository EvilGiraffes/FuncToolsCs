using FuncToolsCs.Internal;

namespace FuncToolsCs.Enumeration;

public static class MaybeIteratorExt
{
    public static IEnumerable<TItem> Enumerable<TItem>(this Maybe<IEnumerable<TItem>> maybe)
        => maybe.UnwrapOr(() => System.Linq.Enumerable.Empty<TItem>());
    public static Maybe<IEnumerable<TReturnItem>> Iterate<TItem, TEnumerable, TReturnItem>(
        this Maybe<TEnumerable> maybe,
        Func<TItem, Maybe<TReturnItem>> map)
        where TReturnItem : notnull
        where TEnumerable : IEnumerable<TItem>
        => maybe.Map(enumerable => MaybeEnumeration.Iterate(enumerable, map));
    public static Maybe<IEnumerable<TReturnItem>> Iterate<TItem, TEnumerable, TReturnItem>(
        this Maybe<TEnumerable> maybe,
        Func<TItem, int, Maybe<TReturnItem>> map)
        where TReturnItem : notnull
        where TEnumerable : IEnumerable<TItem>
    => maybe.Map(enumerable => MaybeEnumeration.Iterate(enumerable, map));
}