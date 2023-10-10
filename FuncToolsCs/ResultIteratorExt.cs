namespace FuncToolsCs;
public static class ResultIteratorEx
{
    public static Result<IEnumerable<TReturnItem>, TError> Iterate<TItem, TError, TEnumerable, TReturnItem>(this Result<TEnumerable, TError> result, Func<TItem, TReturnItem> map)
        where TEnumerable : IEnumerable<TItem>
        where TError : Exception
        => result.Map(enumerable => enumerable.Select(map));
    public static Result<IEnumerable<TReturnItem>, TError> Iterate<TItem, TError, TEnumerable, TReturnItem>(this Result<TEnumerable, TError> result, Func<TItem, int, TReturnItem> map)
        where TEnumerable : IEnumerable<TItem>
        where TError : Exception
        => result.Map(enumerable => enumerable.Select(map));
    public static Result<IEnumerable<TReturnItem>, TError> IterateBind<TItem, TError, TEnumerable, TReturnItem>(this Result<TEnumerable, TError> result, Func<TItem, IEnumerable<TReturnItem>> bind)
        where TEnumerable : IEnumerable<TItem>
        where TError : Exception
        => result.Map(enumerable => enumerable.SelectMany(bind));
    public static Result<IEnumerable<TReturnItem>, TError> IterateBind<TItem, TError, TEnumerable, TReturnItem>(this Result<TEnumerable, TError> result, Func<TItem, int, IEnumerable<TReturnItem>> bind)
        where TEnumerable : IEnumerable<TItem>
        where TError : Exception
        => result.Map(enumerable => enumerable.SelectMany(bind));
    public static Result<IEnumerable<TItem>, TError> IterateFilter<TItem, TError, TEnumerable>(this Result<TEnumerable, TError> result, Func<TItem, bool> filter)
        where TEnumerable : IEnumerable<TItem>
        where TError : Exception
        => result.Map(enumerable => enumerable.Where(filter));
    public static Result<IEnumerable<TReturnItem>, TError> IterateFilterMap<TItem, TError, TEnumerable, TReturnItem>(this Result<TEnumerable, TError> result, Func<TItem, Maybe<TReturnItem>> filter)
        where TEnumerable : IEnumerable<TItem>
        where TError : Exception
        where TReturnItem : notnull
        => result.Map(enumerable =>
        enumerable
        .Select(filter)
        .Where(item => item.IsSome)
        .Select(item => item.UnwrapGuaranteed()));
    public static Result<TAccumelate, TError> IterateFold<TItem, TError, TAccumelate, TEnumerable>(this Result<TEnumerable, TError> result, TAccumelate seed, Func<TAccumelate, TItem, TAccumelate> accumelate)
        where TEnumerable : IEnumerable<TItem>
        where TError : Exception
        where TAccumelate : notnull
        => result.Map(enumerable => enumerable.Aggregate(seed, accumelate));
    public static Result<IEnumerable<TItem>, TError> IterateTake<TItem, TError, TEnumerable>(this Result<TEnumerable, TError> result, int count)
        where TEnumerable : IEnumerable<TItem>
        where TError : Exception
        => result.Map(enumerable => enumerable.Take(count));
    public static Result<IEnumerable<TItem>, TError> IterateTake<TItem, TError, TEnumerable>(this Result<TEnumerable, TError> result, Range range)
        where TEnumerable : IEnumerable<TItem>
        where TError : Exception
        => result.Map(enumerable => enumerable.Take(range));
}
