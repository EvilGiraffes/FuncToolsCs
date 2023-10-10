namespace FuncToolsCs;

static class MaybeIteratorExt
{
    public static Maybe<IEnumerable<TReturnItem>> Iterate<TItem, TEnumerable, TReturnItem>(this Maybe<TEnumerable> maybe, Func<TItem, TReturnItem> map)
        where TEnumerable : IEnumerable<TItem>
        => maybe.Map(enumerable => enumerable.Select(map));
    public static Maybe<IEnumerable<TReturnItem>> Iterate<TItem, TEnumerable, TReturnItem>(this Maybe<TEnumerable> maybe, Func<TItem, int, TReturnItem> map)
        where TEnumerable : IEnumerable<TItem>
        => maybe.Map(enumerable => enumerable.Select(map));
    public static Maybe<IEnumerable<TReturnItem>> IterateBind<TItem, TEnumerable, TReturnItem>(this Maybe<TEnumerable> maybe, Func<TItem, IEnumerable<TReturnItem>> bind)
        where TEnumerable : IEnumerable<TItem>
        => maybe.Map(enumerable => enumerable.SelectMany(bind));
    public static Maybe<IEnumerable<TReturnItem>> IterateBind<TItem, TEnumerable, TReturnItem>(this Maybe<TEnumerable> maybe, Func<TItem, int, IEnumerable<TReturnItem>> bind)
        where TEnumerable : IEnumerable<TItem>
        => maybe.Map(enumerable => enumerable.SelectMany(bind));
    public static Maybe<IEnumerable<TItem>> IterateFilter<TItem, TEnumerable>(this Maybe<TEnumerable> maybe, Func<TItem, bool> filter)
        where TEnumerable : IEnumerable<TItem>
        => maybe.Map(enumerable => enumerable.Where(filter));
    public static Maybe<IEnumerable<TReturnItem>> IterateFilterMap<TItem, TEnumerable, TReturnItem>(this Maybe<TEnumerable> maybe, Func<TItem, Maybe<TReturnItem>> filter)
        where TEnumerable : IEnumerable<TItem>
        where TReturnItem : notnull
        => maybe.Map(enumerable =>
        enumerable
        .Select(filter)
        .Where(item => item.IsSome)
        .Select(item => item.UnwrapGuaranteed()));
    public static Maybe<TAccumelate> IterateFold<TItem, TAccumelate, TEnumerable>(this Maybe<TEnumerable> maybe, TAccumelate seed, Func<TAccumelate, TItem, TAccumelate> accumelate)
        where TEnumerable : IEnumerable<TItem>
        where TAccumelate : notnull
        => maybe.Map(enumerable => enumerable.Aggregate(seed, accumelate));
    public static Maybe<IEnumerable<TItem>> IterateTake<TItem, TEnumerable>(this Maybe<TEnumerable> maybe, int count)
        where TEnumerable : IEnumerable<TItem>
        => maybe.Map(enumerable => enumerable.Take(count));
    public static Maybe<IEnumerable<TItem>> IterateTake<TItem, TEnumerable>(this Maybe<TEnumerable> maybe, Range range)
        where TEnumerable : IEnumerable<TItem>
        => maybe.Map(enumerable => enumerable.Take(range));
}